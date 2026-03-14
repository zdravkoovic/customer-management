namespace Customer.Core.src.Errors;

public sealed class ErrorType
{
    public string Name { get; }
    public int Value { get; }

    private ErrorType(string name, int value)
    {
        Name = name;
        Value = value;
    }

    public static readonly ErrorType Conflict = new("Conflict", 0);
    public static readonly ErrorType NotFound = new("NotFound", 1);
    public static readonly ErrorType BadRequest = new("BadRequest", 2);
    public static readonly ErrorType Validation = new("Validation", 3);
    public static readonly ErrorType Unauthorized = new("Unauthorized", 4);
    public static readonly ErrorType Unexpected = new("Unexpected", 5);

    public static IReadOnlyList<ErrorType> List { get; } = 
            [Conflict, NotFound, BadRequest, Validation, Unexpected];

    public static ErrorType FromName(string name) =>
            List.FirstOrDefault(e => e.Name == name) 
            ?? throw new ArgumentException($"Unknown ErrorType name: {name}");

    public static ErrorType FromValue(int value) =>
            List.FirstOrDefault(e => e.Value == value) 
            ?? throw new ArgumentException($"Unknown ErrorType value: {value}");
    public override string ToString() => Name;

    public override bool Equals(object? obj) => obj is ErrorType other && Value == other.Value;
    public override int GetHashCode() => Value;
}