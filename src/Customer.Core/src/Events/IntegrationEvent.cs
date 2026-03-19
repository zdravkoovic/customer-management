namespace Customer.Core.src.Events;

public abstract class IntegrationEvent : IIntegrationEvent
{
    public int Version { get; set; } = 1; // Default version for the event

    public string EventType { get; set; } // Type of the event

    public Guid Id { get; set; } = Guid.NewGuid(); // Unique identifier for the event

    public DateTimeOffset OccurredOn { get; set; } = DateTimeOffset.UtcNow; // Timestamp of when the event occurred

    public Guid AggregateId { get; set; } // Identifier of the related aggregate

    public IntegrationEvent(Guid aggregateId)
    {
        if (aggregateId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(aggregateId), "AggregateId cannot be empty");
        }

        AggregateId = aggregateId;
        EventType = GetType().Name; // Use the class name as the event type
    }

    public abstract string ToJson();
}