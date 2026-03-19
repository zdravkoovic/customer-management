using CSharpFunctionalExtensions;
using Customer.Application.Customer.Repositories;
using Customer.Core.src.Domain.Common;
using Customer.Core.src.Errors;
using Microsoft.Extensions.Logging;
using IDomainEvent = Customer.Core.src.Events.IDomainEvent;

namespace Customer.Application.Abstractions;

public abstract class CommandHandlerBase<TCommand, TResponse> : ICommandHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
    where TResponse : notnull
{
    private readonly IDomainEventDispatcher _domainEventDispatcher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CommandHandlerBase<TCommand, TResponse>> _logger;

    protected CommandHandlerBase(
        IDomainEventDispatcher domainEventDispatcher,
        IUnitOfWork unitOfWork,
        ILogger<CommandHandlerBase<TCommand, TResponse>> logger
    )
    {
        _domainEventDispatcher = domainEventDispatcher;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    public async Task<Result<TResponse, IDomainError>> Handle(TCommand request, CancellationToken cancellationToken)
    {
        var operationResult = await ExecuteAsync(request, cancellationToken);

        if(!operationResult.IsSuccess)
        {
            return operationResult;
        }

        await SaveIntegrationEventsAsync(cancellationToken);

        await _unitOfWork.SaveChangeAsync(cancellationToken);

        var aggregateRoot = GetAggregateRoot(operationResult);
        _logger.LogInformation($"Aggregate: {aggregateRoot?.ToString()}");
        if(aggregateRoot != null)
        {
            var domainEvents = aggregateRoot.PopDomainEvents();
            await DispatchDomainEventAsync(domainEvents, cancellationToken);
        }
        
        return operationResult;
    }

    protected abstract Task<Result<TResponse, IDomainError>> ExecuteAsync(TCommand request, CancellationToken cancellationToken);

    protected abstract IAggregateRoot? GetAggregateRoot(Result<TResponse, IDomainError> result);

    protected abstract Task SaveIntegrationEventsAsync(CancellationToken cancellationToken);

    protected async Task DispatchDomainEventAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken)
    {
        if(domainEvents == null) return;
        await _domainEventDispatcher.DispatcheAsync(domainEvents, cancellationToken);
    }
}