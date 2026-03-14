using System.Text.Json;
using Confluent.Kafka;
using Customer.Application.Customer.IntegrationEvents;
using Customer.Core.src.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Customer.Infrastructure.src.Messaging.Kafka;

public sealed class KafkaIntegrationEventConsumer : BackgroundService
{
    private readonly IConsumer<string, string> _consumer;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<KafkaIntegrationEventConsumer> _logger;

    public KafkaIntegrationEventConsumer(
        IConsumer<string, string> consumer,
        IServiceProvider serviceProvider,
        ILogger<KafkaIntegrationEventConsumer> logger
    )
    {
        _consumer = consumer;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe("payment-topic");

        while (!stoppingToken.IsCancellationRequested)
        {
            ConsumeResult<string, string> consumeResult;
            try
            {
                consumeResult = _consumer.Consume(stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            
            Console.WriteLine($"Consumed message '{consumeResult.Message.Value}' at: '{consumeResult.TopicPartitionOffset}'.");

            try
            {
                // var envelope = JsonSerializer.Deserialize<IntegrationEventEnvelope<PaymentReceivedEvent>>(
                //     consumeResult.Message.Value
                // );

                var paymentEvent = JsonSerializer.Deserialize<PaymentReceivedEvent>(
                    consumeResult.Message.Value,
                    KafkaJsonSerializerOptions.Default
                );

                var scope = _serviceProvider.CreateScope();
                var handler = scope.ServiceProvider
                    .GetRequiredService<IIntegrationEventHandler<PaymentReceivedEvent>>();
                
                await handler.HandleAsync(paymentEvent!, stoppingToken);
                _consumer.Commit(consumeResult);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling integration event");
                throw;
            }

        }
    }
}