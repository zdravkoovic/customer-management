using AutoMapper;
using CSharpFunctionalExtensions;
using Customer.Application.Abstractions;
using Customer.Application.Customer.Dtos;
using Customer.Application.Customer.Repositories;
using Customer.Core.src.Errors;

namespace Customer.Application.Customer.Queries.GetCustomer;

public class GetCustomerQueryHandler : IQueryHandler<GetCustomerQuery, CustomerDto>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    public GetCustomerQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public async Task<Result<CustomerDto, IDomainError>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.CustomerId);
        if(customer is null)
        {
            return Result.Failure<CustomerDto, IDomainError>(DomainError.NotFound($"Customer with ID: {request.CustomerId} not found"));
        }
        var customerDto = CreateCustomer(customer);
        return Result.Success<CustomerDto, IDomainError>(customerDto);
    }

    private static CustomerDto CreateCustomer(Core.src.CustomerAggregate.Customer customer)
    {
        return new CustomerDto(
            customer.Id.Value,
            customer.Name.FirstName,
            customer.Name.LastName,
            customer.Email.Value
        );
    }
}