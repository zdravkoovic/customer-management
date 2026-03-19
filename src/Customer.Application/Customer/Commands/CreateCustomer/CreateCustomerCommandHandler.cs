using CSharpFunctionalExtensions;
using Customer.Application.Abstractions;
using Customer.Application.Customer.Repositories;
using Customer.Core.src.Domain.Common;
using Customer.Core.src.CustomerAggregate;
using Customer.Core.src.Errors;
using Microsoft.Extensions.Logging;
using CustomerAggregate = Customer.Core.src.CustomerAggregate.Customer;
using IDomainEventDispatcher = Customer.Application.Abstractions.IDomainEventDispatcher;
using Customer.Core.src.Events;
using Customer.Application.Customer.IntegrationEvents;
using Customer.Application.Abstractions.Mappers;

namespace Customer.Application.Customer.Commands.CreateCustomer;

public class CreateCustomerCommandHandler : CommandHandlerBase<CreateCustomerCommand, Guid>
{
    private readonly ICustomerRepository _customerRepository;
    private CustomerAggregate? _createdCustomer;
    private IIntegrationEventResolver _integrationEventResolver;
    private ILogger<CreateCustomerCommandHandler> _logger;
    private readonly IIntegrationEventLog _integrationEventLog;
    public CreateCustomerCommandHandler(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork,
        IIntegrationEventResolver integrationEventResolver,
        ILogger<CreateCustomerCommandHandler> logger,
        IDomainEventDispatcher domainEventDispatcher,
        IIntegrationEventLog integrationEventLog
    ) : base(domainEventDispatcher, unitOfWork, logger)
    {
        _customerRepository = customerRepository;
        _logger = logger;
        _integrationEventLog = integrationEventLog;
        _integrationEventResolver = integrationEventResolver;
    }
    protected override async Task<Result<Guid, IDomainError>> ExecuteAsync(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        _createdCustomer = CustomerAggregate.Create(
            name: CustomerFullName.Create(request.FirstName, request.LastName),
            email: CustomerEmail.Create(request.Email),
            address: CustomerAddress.Create(
                request.Street ?? string.Empty,
                request.HouseNumber ?? string.Empty,
                request.ZipCode ?? string.Empty
            )
        );
        
        if (await _customerRepository.IsExistByCustomerEmailAsync(request.Email))
        {
            return Result.Failure<Guid, IDomainError>(DomainError.Conflict($"Account with the provided email address already exists! You cannot create acount with email address {request.Email}"));
        }

        await _customerRepository.AddAsync(_createdCustomer, cancellationToken);

        return Result.Success<Guid, IDomainError>(_createdCustomer.Id);
    }
    protected override async Task SaveIntegrationEventsAsync(CancellationToken cancellationToken)
    {
        var domainEvents = _createdCustomer?.DomainEvents.ToList();
        if(domainEvents == null || domainEvents.Count() == 0) return;
        var integrationEvents = _integrationEventResolver.Resolve(domainEvents);
        await _integrationEventLog.AddIntegrationEventsAsync([.. integrationEvents], cancellationToken);
    }
    protected override IAggregateRoot? GetAggregateRoot(Result<Guid, IDomainError> result)
    {
        return _createdCustomer;
    }
}