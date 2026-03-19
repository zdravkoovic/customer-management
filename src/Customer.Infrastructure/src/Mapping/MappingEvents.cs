using Customer.Core.src.Events;
using Customer.Infrastructure.src.Persistance.Model;

namespace Customer.Infrastructure.src.Mapping;

public static class MappingEvents
{
    // This class is responsible for mapping domain events to integration events.
    // It can be used to map domain events to integration events that will be published to a message broker.
    // The mapping can be done using a library like AutoMapper or manually by creating a new instance of the integration event and copying the properties from the domain event.
    public static OutboxEvent Map(IIntegrationEvent @event)
    {
        var integrationEvent = new OutboxEvent()
        {
            AggregateId = @event.AggregateId,
            Payload = @event.ToJson(),
            Type = @event.EventType,
            OccurredAt = @event.OccurredOn.UtcDateTime
        };

        return integrationEvent;
    }
}