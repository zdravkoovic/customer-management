namespace Customer.Application.Customer.Repositories;

public interface IIntegrationEventLog 
{
    Task<bool> HasProcessedAsync(Guid eventId);
    Task MarkAsProcessedAsync(Guid eventId);
    
}