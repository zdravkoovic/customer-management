namespace Customer.Application.Customer.Repositories;

public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangeAsync(CancellationToken cancellationToken = default);
    // Task<int> SaveChangeAsync(Core.src.CustomerAggregate.Customer customer, CancellationToken cancellationToken = default);
}