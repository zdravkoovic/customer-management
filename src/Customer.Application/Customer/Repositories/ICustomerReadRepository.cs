namespace Customer.Application.Customer.Repositories;

public interface ICustomerReadRepository
{
    Task<Core.src.CustomerAggregate.Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    // Task<PagedResult<CustomerListDto>> SearchAsync(string query, int page, int size, CancellationToken cancellationToken = default);
}