using Customer.Core.src.Events;
using Customer.Core.src.Events.Decorators;

namespace Customer.Core.src.CustomerAggregate.Events;

[AggregateType("customer-service.customer")]
public abstract class BaseCustomerDomainEvent(Guid aggregateId, DateTimeOffset? occuredOnUtc = null)
    : DomainEvent(aggregateId, occuredOnUtc ?? DateTimeOffset.UtcNow) {}