namespace Customer.Core.src.Domain.Common;

public abstract class Entity<TModel>
{
    public Id<TModel> Id { get; }
    public DateTimeOffset CreatedAtUtc { get; }
    public DateTimeOffset? LastModifiedAtUtc { get; set; }

    protected Entity(Id<TModel> id)
    {
        Id = id;
        CreatedAtUtc = DateTimeOffset.UtcNow;
        LastModifiedAtUtc = null;
    }

    protected Entity() : this(Id<TModel>.New()) {}
    public override bool Equals(object? obj)
    {
        if(obj is Entity<TModel> entity)
        {
            return entity.Id == Id;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}