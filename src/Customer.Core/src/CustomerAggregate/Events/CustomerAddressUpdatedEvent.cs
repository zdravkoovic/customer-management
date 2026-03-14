namespace Customer.Core.src.CustomerAggregate.Events;

public sealed class CustomerAddressUpdatedEvent : BaseCustomerDomainEvent
{
    public CustomerAddressUpdatedEvent(Customer customer) : base(customer.Id)
    {
        Customer = customer;
    }
    public Customer Customer { get; init; }
}