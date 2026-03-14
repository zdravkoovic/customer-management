using System.Runtime.Serialization;

namespace Customer.Core.src.Domain.Common.Exceptions;

// [Serializable]
public class ValidationException : Exception
{
    public IReadOnlyList<string> Errors { get; }

    public ValidationException(IEnumerable<string> errors)
        : base("Validation failed.")
    {
        Errors = errors?.ToList() ?? [];
    }

    public ValidationException(string error)
        : base(error)
    {
        Errors = [];
    }

    public ValidationException(string message, Exception inner)
        : base(message, inner)
    {
        Errors = [inner.Message];
    }

}