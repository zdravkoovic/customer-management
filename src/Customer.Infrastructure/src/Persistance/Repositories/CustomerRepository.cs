namespace Customer.Infrastructure.src.Persistance.Repositories;

using Customer.Core.src.CustomerAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Customer.Application.Customer.Repositories;
using AutoMapper;

public class CustomerRepository(AppDbContext appDbContext, ILogger<CustomerRepository> logger, IMapper mapper) : ICustomerRepository
{
    private readonly AppDbContext _dbContext = appDbContext;
    private readonly ILogger<CustomerRepository> _logger = logger;
    private readonly IMapper _mapper = mapper;

    public async Task AddAsync(Customer entity, CancellationToken cancellationToken)
    {
        var customerEntity = _mapper.Map<Model.Customer>(entity);
        await _dbContext.AddAsync(customerEntity, cancellationToken);
    }

    public Task DeleteAsync(Guid id)
    {
        return Task.FromResult(_dbContext.Remove(id));
    }

    public async Task<Customer?> FindByEmailAsync(string email)
    {
        var customer = _dbContext.Customers.AsNoTracking().Where(x => x.Email == email);
        return _mapper.Map<Customer>(customer);
    }

    public Task<IEnumerable<Customer>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Customer?> GetByIdAsync(Guid id)
    {
        var customer = await _dbContext.Customers.FindAsync(id);
        if(customer == null) return null;
        return CreateCustomer(customer);
    }

    public async Task<bool> IsExistByCustomerIdAsync(Guid id)
    {
        return (await _dbContext.Customers.FirstOrDefaultAsync(x => x.Id == id)) != null;
    }

    public async Task<bool> IsExistsAsync(Guid id)
    {
        return (await _dbContext.Customers.FirstOrDefaultAsync(x => x.Id == id)) != null;
    }

    public Task SendEmailForSuccessfullPayment()
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Customer entity)
    {
        return Task.FromResult(_dbContext.Update(entity));
    }

    private static Customer CreateCustomer(Model.Customer customer)
    {
        return Customer.Create(
            CustomerFullName.Create(customer.FirstName,customer.LastName),
            CustomerEmail.Create(customer.Email),
            CustomerAddress.Create(street: customer.Street ?? string.Empty,
                                   houseNumber: customer.HouseNumber ?? string.Empty,
                                   zipCode: customer.ZipCode ?? string.Empty)
        );
    }

    public async Task<bool> IsExistByCustomerEmailAsync(string Email)
    {
        return await _dbContext.Customers
            .AsNoTracking()
            .Where(x => x.Email == Email)
            .FirstOrDefaultAsync() != null;
    }
}