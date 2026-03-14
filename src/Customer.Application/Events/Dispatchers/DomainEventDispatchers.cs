using Customer.Core.src.Domain.Common;
using Customer.Core.src.Events;
using MediatR;

namespace Customer.Application.Events.Dispatchers;

public class DomainEventDispatchers : Abstractions.IDomainEventDispatcher
{
    private readonly IMediator _mediator;
    public DomainEventDispatchers(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task DispatcheAsync(IEnumerable<IDomainEvent> initialEvents, CancellationToken cancellationToken)
    {
        var eventQueue = new Queue<IDomainEvent>(initialEvents);
        while(eventQueue.Count > 0)
        {
            var currentEvent = eventQueue.Dequeue();

            await _mediator.Publish(currentEvent, cancellationToken);

            if(currentEvent is IAggregateRoot aggregateRoot)
            {
                var additionalEvents = aggregateRoot.PopDomainEvents();
                foreach (var additionalEvent in additionalEvents)
                {
                    eventQueue.Enqueue(additionalEvent);
                }
            }
        }
    }
}