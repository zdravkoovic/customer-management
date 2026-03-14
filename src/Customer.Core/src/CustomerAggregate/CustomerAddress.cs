namespace Customer.Core.src.CustomerAggregate;

public sealed class CustomerAddress(string street, string houseNumber, string zipCode)
{
    public string Street { get; private set; } = street;
    public string HouseNumber { get; private set; } = houseNumber;
    public string ZipCode { get; private set; } = zipCode;

    public static CustomerAddress Create(
        string street,
        string houseNumber,
        string zipCode
    )
    {
        return new CustomerAddress(street, houseNumber, zipCode);
    }
}