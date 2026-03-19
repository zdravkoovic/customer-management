using System.Text.Json;
using Confluent.Kafka;
using Customer.Application.Abstractions.Messaging;
using Customer.Core.src.Events;
using Customer.Infrastructure.src.Persistance;
using Customer.Infrastructure.src.Persistance.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Customer.Infrastructure.src.Messaging.Kafka;

public class KafkaIntegrationEventPublisher : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<KafkaIntegrationEventPublisher> _logger;
    private readonly IProducer<string, string> _producer;
    private readonly Dictionary<string, string> _topics;

    private const int BatchSize = 20;
    private static readonly TimeSpan PollDelayWhenIdle = TimeSpan.FromSeconds(1);
    private static readonly TimeSpan DefaultLockDuration = TimeSpan.FromSeconds(30);
    private const int MaxAttempts = 5;

    public KafkaIntegrationEventPublisher(
        IServiceProvider serviceProvider,
        ILogger<KafkaIntegrationEventPublisher> logger,
        IProducer<string, string> producer,
        Dictionary<string, string> topics
    )
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _producer = producer;
        _topics = topics;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("KafkaIntegrationEventPublisher started.");
        while(!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var now = DateTime.UtcNow;
            
            var candidates = await db.OutboxEvents
                .Where(o => o.ProcessedAt == null && (o.LockedUntil == null || o.LockedUntil < now))
                .OrderBy(o => o.OccurredAt)
                .Take(BatchSize * 3)
                .ToListAsync(stoppingToken);
            
            if(candidates.Count() == 0)
            {
                await Task.Delay(PollDelayWhenIdle, stoppingToken);
                continue;
            }

            var claimed = new List<OutboxEvent>();
            foreach(var candidate in candidates)
            {
                if(claimed.Count >= BatchSize) break;

                var newLockUntil = DateTime.UtcNow.Add(DefaultLockDuration);

                var rows = await db.Database.ExecuteSqlInterpolatedAsync($@"
                    UPDATE ""OutboxEvents""
                    SET ""LockedUntil"" = {newLockUntil}, ""AttemptCount"" = ""AttemptCount"" + 1
                    WHERE ""Id"" = {candidate.Id} AND ""ProcessedAt"" IS NULL AND (""LockedUntil"" IS NULL OR ""LockedUntil"" < {now})",
                    cancellationToken: stoppingToken
                );
                if(rows == 1)
                {
                    var claimedRow = await db.OutboxEvents.FirstOrDefaultAsync(o => o.Id == candidate.Id, stoppingToken);
                    if(claimedRow != null) claimed.Add(claimedRow);
                }
            }

            if(claimed.Count == 0)
            {
                await Task.Delay(TimeSpan.FromMicroseconds(200), stoppingToken);
                continue;
            }

            foreach(var ev in claimed)
            {
                try
                {
                    if(ev.AttemptCount > MaxAttempts)
                    {
                        ev.Error = $"Exceeded max attempts ({ev.AttemptCount}).";
                        ev.LockedUntil = null;
                        db.OutboxEvents.Update(ev);
                        await db.SaveChangesAsync(stoppingToken);
                        continue;
                    }

                    if (!_topics.TryGetValue(ev.Type, out var topic))
                    {
                        _logger.LogWarning("No topic mapping found for {EventType}", ev.Type);
                        continue;
                    }

                    var message = new Message<string, string>
                    {
                        Key = ev.Id.ToString(),
                        Value = ev.Payload,
                        Headers = new Headers
                        {
                            {"message-id", System.Text.Encoding.UTF8.GetBytes(ev.Id.ToString())},
                            {"event-type", System.Text.Encoding.UTF8.GetBytes(ev.Type)}
                        }
                    };

                    var delivery = await _producer.ProduceAsync(topic, message, stoppingToken);

                    ev.ProcessedAt = DateTime.UtcNow;
                    ev.Error = null;
                    ev.LockedUntil = null;
                    await db.SaveChangesAsync(stoppingToken);

                    _logger.LogInformation(
                        "Published OutboxEvent {Id} to {Topic} partition {Partition} offset {Offset}",
                        ev.Id, topic, delivery.Partition.Value, delivery.Offset.Value);
                }
                catch(ProduceException<string, string> pex)
                {
                    ev.Error = pex.Error.Reason;
                    ev.LockedUntil = DateTime.UtcNow.AddSeconds(Math.Min(30, 2 * ev.AttemptCount));
                    db.OutboxEvents.Update(ev);
                    await db.SaveChangesAsync(stoppingToken);
                    _logger.LogError(pex, "Kafka produce failed for OutboxEvent {Id}", ev.Id);
                }
                catch(Exception ex)
                {
                    ev.Error = ex.Message;
                    ev.LockedUntil = DateTime.UtcNow.AddSeconds(Math.Min(30, 2 * ev.AttemptCount));
                    await db.SaveChangesAsync(stoppingToken);
                    _logger.LogError(ex, "Unexpected error publishing OutboxEvent {Id}", ev.Id);
                }
            }
        }
    }
}