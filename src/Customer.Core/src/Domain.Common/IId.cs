namespace Customer.Core.src.Domain.Common;

public interface IId : IComparable, IComparable<IId>, IComparable<Guid>, IEquatable<IId>
{
    Guid Value { get; }
}