using Customer.Core.src.CustomerAggregate.Events;
using Customer.Core.src.Domain.Common;

namespace Customer.Core.src.CustomerAggregate;

public sealed class Customer(CustomerFullName name, CustomerEmail email, CustomerAddress? address = null) : AggregateRoot<Customer>
{
    public CustomerFullName Name { get; private set; } = name;
    public CustomerEmail Email { get; private set; } = email;
    public CustomerAddress? Address { get; private set; } = address;
    public void UpdateEmail(CustomerEmail newEmail)
    {
        Email = newEmail ?? throw new ArgumentNullException(nameof(newEmail));
        RaiseDomainEvent(new CustomerEmailUpdateEvent(this));
    }

    public void UpdateAddress(CustomerAddress newAddress)
    {
        Address = newAddress ?? throw new ArgumentNullException(nameof(newAddress));
        LastModifiedAtUtc = DateTime.UtcNow;
        RaiseDomainEvent(new CustomerAddressUpdatedEvent(this));
    }

    public Customer UpdateName(CustomerFullName newName)
    {
        if (Name == newName) return this;
        Name = newName ?? throw new ArgumentNullException(nameof(newName));
        LastModifiedAtUtc = DateTime.UtcNow;
        RaiseDomainEvent(new CustomerFullnameUpdateEvent(this));

        return this;
    }

    public static Customer Create(CustomerFullName name, CustomerEmail email, CustomerAddress? address = null)
    {
        var customer = new Customer(name, email, address);
        customer.RaiseDomainEvent(new CustomerCreatedEvent(customer));
        return customer;
    }

}