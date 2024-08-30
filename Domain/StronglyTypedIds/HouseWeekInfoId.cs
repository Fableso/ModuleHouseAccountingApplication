using Domain.StronglyTypedIds.Interface;

namespace Domain.StronglyTypedIds;

public readonly record struct HouseWeekInfoId(long Value) : IStronglyTypedId<long>;