namespace Customer.Core.src.CustomerAggregate.Events;

public sealed class CustomerFullnameUpdateEvent : BaseCustomerDomainEvent
{
    public CustomerFullnameUpdateEvent(Customer customer) : base(customer.Id)
    {
        Customer = customer;
    }
    public Customer Customer { get; init; }
}