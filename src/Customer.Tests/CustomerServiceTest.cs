namespace Customer.Tests;

using Customer.Core.src.Interfaces;
using Customer.Core.src.Domain.Entities;
using Customer.Core.src.Services;
using Moq;
using FluentAssertions;
using Customer.Core.src.Domain.ValueObjects;

public class CustomerServiceTest
{
    [Fact]
    public void GetCustomer_ReturnsCustomer_WhenExists()
    {
        // Arrange
        var id = Guid.NewGuid();

        var repoMock = new Mock<ICustomerRepository>();
        repoMock.Setup(r => r.GetById(id))
                .Returns(new Customer(id, "John Doe", new EmailAddress("johndoe@gmail.com"), null));
        
        var service = new CustomerService(repoMock.Object);

        // Act
        var result = service.GetCustomer(id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
        result.Name.Should().Be("John Doe");
    }

    [Fact]
    public void GetCustomer_ReturnsNull_WhenNotExists()
    {
        // Arrange
        var id = Guid.NewGuid();

        var repoMock = new Mock<ICustomerRepository>();
        repoMock.Setup(r => r.GetById(id))
                .Returns((Customer?)null);
        
        var service = new CustomerService(repoMock.Object);

        // Act
        var result = service.GetCustomer(id);

        result.Should().BeNull();
    }
}
