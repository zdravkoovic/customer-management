using Customer.Core.src.Domain.Common;
using Customer.Core.src.Events;
using CustomerAggregate = Customer.Core.src.CustomerAggregate.Customer;

namespace Customer.Application.Events.Integrations;

public sealed class CustomerIntegrationEvent : IntegrationEvent
{
    public CustomerIntegrationEvent(Id<CustomerAggregate> id) : base(id)
    {
        CustomerId = id;
    }

    public Guid CustomerId { get; }

    public override string ToJson()
    {
        throw new NotImplementedException();
    }
}