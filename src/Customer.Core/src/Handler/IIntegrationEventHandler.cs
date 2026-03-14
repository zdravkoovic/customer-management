using Customer.Core.src.Events;

namespace Customer.Core.src.Handler;

public interface IIntegrationEventHandler<T> where T : IntegrationEvent
{
    Task HandleAsync(T @event, CancellationToken cancellationToken = default);
}