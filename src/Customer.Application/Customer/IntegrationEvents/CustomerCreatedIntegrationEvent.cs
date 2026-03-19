using Customer.Core.src.Events;

namespace Customer.Application.Customer.IntegrationEvents;

public class CustomerCreatedIntegrationEvent : IntegrationEvent
{
    public string Name { get; set; }
    public string Email { get; set; }

    public CustomerCreatedIntegrationEvent(IDomainEvent @event, string name, string email) : base(@event.AggregateId)
    {
        Name = name;
        Email = email;
        OccurredOn = @event.OccurredOnUtc;
        EventType = @event.GetType().Name;
    }

    public override string ToJson()
    {
        return System.Text.Json.JsonSerializer.Serialize(this);
    }
}