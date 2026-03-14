namespace Customer.Core.src.CustomerAggregate.Events;

public sealed class CustomerEmailUpdateEvent : BaseCustomerDomainEvent
{
    public CustomerEmailUpdateEvent(Customer customer) : base(customer.Id)
    {
        Customer = customer;
    }

    public Customer Customer { get; init; }
}