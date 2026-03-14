using Customer.Core.src.Events;

namespace Customer.Application.Abstractions.Messaging;

public interface IIntegrationEventPublisher
{
    Task PublishAsync<T>(T @event) where T : IntegrationEvent;
}