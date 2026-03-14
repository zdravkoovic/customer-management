using System.Reflection;
using Customer.Core.src.Events.Decorators;

namespace Customer.Core.src.Events;

public class DomainEvent : IDomainEvent
{
    public int Version {get; set;} = 1;

    public string? AggregateType { get; set; }

    public string? EventType { get; set; }

    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTimeOffset OccurredOnUtc { get; set; }

    public Guid AggregateId { get; set; }

    public string? TraceInfo { get; set; }

    public DomainEvent() {}

    public DomainEvent(Guid aggregateId, DateTimeOffset occuredOnUtc)
    {
        AggregateId = aggregateId;
        OccurredOnUtc = occuredOnUtc;
        AggregateType = GetAggregateType(GetType() ?? throw new InvalidOperationException("Aggregate type cannot be null."));
        EventType = GetEventType(this);
    }

    public static string GetAggregateType<TEvent>() where TEvent : IDomainEvent => 
        GetAggregateType(typeof(TEvent));
    public static string GetAggregateType(Type eventType)
    {
        var attribute = eventType.GetCustomAttribute<AggregateTypeAttribute>();
        return attribute?.AggregateType ?? string.Empty;
    }

    public static string GetEventType(IDomainEvent @event) => GetEventType(@event.GetType(), @event.AggregateType);
    public static string GetEventType(Type eventType, string? prefix = null)
    {
        prefix ??= GetEventType(eventType);
        return $"{prefix}.{eventType.Name}";
    }
    
}