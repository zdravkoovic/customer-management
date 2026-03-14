namespace Customer.Core.src.Errors;

public record DomainError : IDomainError
{
    public static DomainError Conflict(string? message = "The data provided conflicts with existing data.") =>
        new(message, ErrorType.Conflict);

    public static DomainError NotFound(string? message = "The requested resource was not found.") =>
        new(message, ErrorType.NotFound);
    
    public static DomainError BadRequest(string? message = "Invalid request or parameters.") =>
        new(message, ErrorType.BadRequest);

    public static DomainError Validation(string? message = "One or more validation errors occurred.", List<string>? errors = null) =>
        new(message, ErrorType.Validation, errors);
    
    public static DomainError Unauthorized(string? message = "Unauthorized access.") =>
        new(message, ErrorType.Unauthorized);
    
    public static DomainError Unexpected(string? message = "An unexpected error occurred.") =>
        new(message, ErrorType.Unexpected);

    // Ctor is private to enforce the use of static factory methods
    private DomainError(string? message, ErrorType   type, List<string>? errors = null)
    {
        ErrorMessage = message;
        ErrorType = type;
        Errors = errors ?? [];
    }
    public string? ErrorMessage { get; init; }
    public ErrorType ErrorType { get; init; }
    public List<string>? Errors { get; init; }
}