using Customer.Core.src.Domain.Common.Extensions;
using Customer.Core.src.Events;

namespace Customer.Core.src.Domain.Common;

public interface IAggregateRoot
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    IReadOnlyCollection<IDomainEvent> PopDomainEvents();
    void ClearEvents();
}

public abstract class AggregateRoot<TModel> : Entity<TModel>, IAggregateRoot
{
    private readonly IList<IDomainEvent> _domainEvents = [];
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void ClearEvents()
    {
        _domainEvents.Clear();
    }

    public IReadOnlyCollection<IDomainEvent> PopDomainEvents()
    {
        var events = _domainEvents.ToList();
        ClearEvents();
        return events;
    }

    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        domainEvent.EnsureNonNull();
        _domainEvents.Add(domainEvent);
    }
}