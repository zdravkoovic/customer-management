namespace Customer.Application.Customer.Repositories;

public interface ICustomerRepository : IRepository<Core.src.CustomerAggregate.Customer>
{
    Task<Core.src.CustomerAggregate.Customer?> FindByEmailAsync(string email);
    Task<bool> IsExistByCustomerIdAsync(Guid id);
    Task<bool> IsExistByCustomerEmailAsync(string Email);
    Task SendEmailForSuccessfullPayment();
}