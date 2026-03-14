namespace Customer.Application.Customer.Dtos;

public class CustomerDto(Guid id, string firstName, string lastName, string email, string? street = null, string? city = null, string? state = null, string? postalCode = null, string? country = null)
{
    public Guid Id { get; set; } = id;
    public string Firstname { get; set; } = firstName;
    public string Lastname { get; set; } = lastName;
    public string Email { get; set; } = email;
    public string Street { get; private set; } = street ?? string.Empty;
    public string City { get; private set; } = city ?? string.Empty;
    public string State { get; private set; } = state ?? string.Empty;
    public string PostalCode { get; private set; } = postalCode ?? string.Empty;
    public string Country { get; private set; } = country ?? string.Empty;
}