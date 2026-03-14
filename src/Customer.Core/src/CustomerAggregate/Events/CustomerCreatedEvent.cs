namespace Customer.Core.src.CustomerAggregate.Events;

public sealed class CustomerCreatedEvent : BaseCustomerDomainEvent
{
    public CustomerCreatedEvent(Customer customer)
        :base(customer.Id)
    {
        Customer = customer;
    }
    
    public Customer Customer { get; }
}