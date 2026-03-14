using Customer.Core.src.Domain.ValueObjects;
using Customer.Infrastructure.src.Persistance;
using Customer.Infrastructure.src.Persistance.Repositories;
using Customer.Infrastructure.src.Services;
using Customer.Tests.Infrastructure.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace Customer.Tests.Infrastructure;

public class CustomerRepositoryTests : IClassFixture<PostgresContainerFixture>
{
    private readonly AppDbContext _db;
    private readonly CustomerRepository _repo;

    public CustomerRepositoryTests(PostgresContainerFixture dbFixture)
    {
        _db = dbFixture.DbContext;
        _repo = new CustomerRepository(_db, NullLogger<CustomerRepository>.Instance, new CustomerMapper());
    }

    [Fact]
    public async Task AddAsync_ShouldPersistCustomer()
    {
        // Arrange
        var customer = new Core.src.Domain.Entities.Customer(Guid.Empty, "John Doe", EmailAddress.Create("john@test.com"), DateTime.UtcNow);
        
        // Act
        var result = await _repo.AddCustomerAsync(customer);

        // Assert
        var entity = await _db.Customers
            .AsNoTracking()
            .SingleOrDefaultAsync(c => c.Email == "john@test.com");

        Assert.NotNull(entity);
        Assert.Equal(result.Id, entity.Id);
    }

    [Fact]
    public async Task AddAsync_DuplicateEmail_ShouldThrow()
    {
        var customer1 = new Core.src.Domain.Entities.Customer(
            Guid.NewGuid(),
            "John",
            EmailAddress.Create("dup@test.com"),
            DateTime.UtcNow
        );

        var customer2 = new Core.src.Domain.Entities.Customer(
            Guid.NewGuid(),
            "Jane",
            EmailAddress.Create("dup@test.com"),
            DateTime.UtcNow
        );

        await _repo.AddCustomerAsync(customer1);

        // _db.ChangeTracker.Clear();

        await Assert.ThrowsAsync<DbUpdateException>(() =>
            _repo.AddCustomerAsync(customer2));
    }
}