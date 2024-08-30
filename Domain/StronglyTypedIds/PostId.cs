using Domain.StronglyTypedIds.Interface;

namespace Domain.StronglyTypedIds;

public readonly record struct PostId(long Value) : IStronglyTypedId<long>;