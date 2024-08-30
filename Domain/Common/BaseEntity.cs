namespace Domain.Common;

public abstract class BaseEntity<TEntityId>
{
    protected BaseEntity() { }
    protected BaseEntity(TEntityId id)
    {
        Id = id;
    }
    public TEntityId Id { get; init; }
}