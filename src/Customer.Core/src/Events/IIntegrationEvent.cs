namespace Customer.Core.src.Events;

public interface IIntegrationEvent
{
    int Version { get; }
    string EventType { get; }
    Guid Id { get; }
    DateTimeOffset OccurredOn { get; }
    Guid AggregateId { get; }

    public string ToJson();
}