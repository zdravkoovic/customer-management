using Customer.Core.src.Events;

namespace Customer.Application.Abstractions.Messaging;

public interface IIntegrationEventMapper
{
    bool CanMap(IDomainEvent domainEvent);
    IIntegrationEvent? Map(IDomainEvent domainEvent);
}

public interface IIntegrationEventMapper<TDomain> where TDomain : IDomainEvent
{
    IIntegrationEvent Map(TDomain domainEvent);
}