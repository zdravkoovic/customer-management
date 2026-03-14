using Customer.Infrastructure.src.Persistance;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace Customer.Tests.Infrastructure.Fixtures;

public class PostgresContainerFixture: IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres;
    public AppDbContext DbContext { get; private set; } = null!;

    public PostgresContainerFixture()
    {
        _postgres = new PostgreSqlBuilder()
            .WithImage("postgres:latest")
            .WithDatabase("customer_test")
            .WithUsername("test")
            .WithPassword("test")
            .Build();
    }
    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(_postgres.GetConnectionString())
            .Options;
            
        DbContext = new AppDbContext(options);
        await DbContext.Database.MigrateAsync();
    }
    public async Task DisposeAsync()
    {
        await DbContext.Database.EnsureDeletedAsync();
        await DbContext.DisposeAsync();
    }

}