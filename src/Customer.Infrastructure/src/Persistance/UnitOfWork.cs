using Customer.Application.Customer.Repositories;

namespace Customer.Infrastructure.src.Persistance;

public class UnitOfWork(AppDbContext dbContext) : IUnitOfWork
{
    private readonly AppDbContext _dbContext = dbContext;
    public void Dispose() => _dbContext.Dispose();

    public async Task<int> SaveChangeAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}