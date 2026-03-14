using Customer.Application.Abstractions;

namespace Customer.Application.Customer.Commands.CreateCustomer;

public record CreateCustomerCommand(
    string FirstName, 
    string LastName, 
    string Email,
    string? Street = null,
    string? HouseNumber = null,
    string? ZipCode = null
) : ICommand<Guid>;
