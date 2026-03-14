using Customer.Application.Abstractions;
using Customer.Application.Customer.Dtos;

namespace Customer.Application.Customer.Queries.GetCustomer;

public record GetCustomerQuery(Guid CustomerId) : IQuery<CustomerDto>;