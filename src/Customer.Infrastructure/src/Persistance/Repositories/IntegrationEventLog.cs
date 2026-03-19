using Customer.Application.Customer.Repositories;
using Customer.Core.src.Events;
using Customer.Infrastructure.src.Mapping;
using Customer.Infrastructure.src.Persistance;

namespace Customer.Infrastructure.Persistance.Repositories;

public class IntegrationEventLog : IIntegrationEventLog
{
    private readonly AppDbContext _dbContext;

    public IntegrationEventLog(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddIntegrationEventsAsync(List<IIntegrationEvent> integrationEvents, CancellationToken cancellationToken)
    {
        var outboxEvents = integrationEvents.Select(MappingEvents.Map);
        await _dbContext.OutboxEvents.AddRangeAsync(outboxEvents, cancellationToken);
    }

    public Task<bool> HasProcessedAsync(Guid eventId)
    {
        throw new NotImplementedException();
    }

    public Task MarkAsProcessedAsync(Guid eventId)
    {
        throw new NotImplementedException();
    }
}