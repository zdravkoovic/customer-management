namespace Customer.Core.src.CustomerAggregate;

public sealed class CustomerFullName
{
    public string FirstName { get; }
    public string LastName { get; }

    public CustomerFullName(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new ArgumentException("First name cannot be empty.", nameof(firstName));
        }
        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new ArgumentException("Last name cannot be empty.", nameof(lastName));
        }

        FirstName = firstName;
        LastName = lastName;
    }

    public static CustomerFullName Create(string firstName, string lastName)
    {
        return new CustomerFullName(firstName, lastName);
    }

    public override string ToString() => $"{FirstName} {LastName}";
}