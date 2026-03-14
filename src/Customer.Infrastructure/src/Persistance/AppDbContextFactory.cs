using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Customer.Infrastructure.src.Persistance;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var cs = Environment.GetEnvironmentVariable("CUSTOMER_CONNECTION")
            ?? "Host=localhost;Port=5432;Database=customers;Username=johnny;Password=johnny123;";

        var options = new DbContextOptionsBuilder<AppDbContext>();
        options.UseNpgsql(cs);

        return new AppDbContext(options.Options);
    }
}