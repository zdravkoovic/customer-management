using Customer.Application.Abstractions.Messaging;
using Customer.Application.Customer.IntegrationEvents;
using Customer.Core.src.CustomerAggregate.Events;
using Customer.Core.src.Events;

namespace Customer.Application.Events.Integrations.Mappers;

public class CustomerCreatedMapper : IIntegrationEventMapper<CustomerCreatedEvent>
{
    public IIntegrationEvent Map(CustomerCreatedEvent domainEvent) => new CustomerCreatedIntegrationEvent(domainEvent, domainEvent.Customer.Name.ToString(), domainEvent.Customer.Email.ToString());
}