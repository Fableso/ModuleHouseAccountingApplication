using Domain.StronglyTypedIds.Interface;

namespace Domain.StronglyTypedIds;

public readonly record struct HousePostId(long Value) : IStronglyTypedId<long>;