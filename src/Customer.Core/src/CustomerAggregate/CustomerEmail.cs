namespace Customer.Core.src.CustomerAggregate;

public sealed class CustomerEmail
{
    public string Value { get; }

    public CustomerEmail(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Email address cannot be empty.", nameof(value));
        }
        Value = value;
        
    }

    public static CustomerEmail Create(string email)
    {
        // Basic validation for example purposes
        if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
        {
            throw new ArgumentException("Invalid email address.", nameof(email));
        }

        return new CustomerEmail(email);
    }

    public override string ToString() => Value;
}