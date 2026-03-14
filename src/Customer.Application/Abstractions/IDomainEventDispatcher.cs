using Customer.Core.src.Events;

namespace Customer.Application.Abstractions;

public interface IDomainEventDispatcher
{
    Task DispatcheAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken);
}