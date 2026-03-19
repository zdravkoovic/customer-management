using Customer.Core.src.Events;

namespace Customer.Application.Abstractions.Mappers;

public interface IIntegrationEventResolver
{
    IReadOnlyCollection<IIntegrationEvent> Resolve(IEnumerable<IDomainEvent> domainEvents);
}