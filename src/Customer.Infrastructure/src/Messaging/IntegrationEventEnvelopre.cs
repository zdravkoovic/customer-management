using System.Text.Json;
using Customer.Core.src.Events;

namespace Customer.Infrastructure.src.Messaging;

public class IntegrationEventEnvelope<TEvent> where TEvent : IIntegrationEvent
{
    public Type EventType { get; set; } = typeof(TEvent);
    public int Version { get; set; } = 1;
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTimeOffset OccurredOn { get; set; }
    public Guid AggregateId { get; set; }
    public JsonElement Data { get; set; }
}