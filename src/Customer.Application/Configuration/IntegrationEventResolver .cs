using Customer.Application.Abstractions.Mappers;
using Customer.Application.Abstractions.Messaging;
using Customer.Core.src.Events;

namespace Customer.Application.Configuration;

public class IntegrationEventResolver : IIntegrationEventResolver
{
    private readonly IServiceProvider _serviceProvider;
    public IntegrationEventResolver(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public IReadOnlyCollection<IIntegrationEvent> Resolve(IEnumerable<IDomainEvent> domainEvents)
    {
        var result = new List<IIntegrationEvent>();
        
        foreach(var domainEvent in domainEvents)
        {
            var domainEventType = domainEvent.GetType();

            var mapperType = typeof(IIntegrationEventMapper<>)
                .MakeGenericType(domainEventType);
            
            var mapper = _serviceProvider.GetService(mapperType);

            if(mapper == null)
            {
                throw new InvalidOperationException($"No mapper found for domain event type {domainEventType.Name}");
            }

            var integrationEvent = (IIntegrationEvent)((dynamic)mapper).Map((dynamic)domainEvent);

            result.Add(integrationEvent);
        }
        
        return result;
    }
}