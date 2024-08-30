namespace Domain.StronglyTypedIds.Interface;

public interface IStronglyTypedId<T>
{
    T Value { get; }
}