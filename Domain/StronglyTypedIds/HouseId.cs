using Domain.StronglyTypedIds.Interface;

namespace Domain.StronglyTypedIds;

public readonly record struct HouseId(string Value) : IStronglyTypedId<string>;