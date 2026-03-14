using Customer.Core.src.Events;

namespace Customer.Application.Customer.IntegrationEvents;

public interface IIntegrationEventHandler<TEvent> where TEvent : IIntegrationEvent
{
    public Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
}