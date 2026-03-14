using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Customer.Infrastructure.src.Configurations;

using Customer.Infrastructure.src.Persistance.Model;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.FirstName)
            .IsRequired();
        builder.Property(c => c.LastName)
            .IsRequired();
        builder.Property(c => c.Email)
            .IsRequired();
        builder.Property(c => c.HouseNumber);
        builder.Property(c => c.ZipCode);
        builder.Property(c => c.CreatedAt)
            .IsRequired();
        builder.Property(c => c.UpdatedAt);
        builder.Property(c => c.Street);
    }
}