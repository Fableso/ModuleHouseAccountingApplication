using Domain.StronglyTypedIds.Interface;

namespace Domain.StronglyTypedIds;

public readonly record struct WeekMarkId(Guid Value) : IStronglyTypedId<Guid>;