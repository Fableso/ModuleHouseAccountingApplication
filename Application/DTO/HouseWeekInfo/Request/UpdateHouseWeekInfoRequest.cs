using Domain.Enums;
using Domain.StronglyTypedIds;

namespace Application.DTO.HouseWeekInfo.Request;

public sealed class UpdateHouseWeekInfoRequest
{
    public HouseWeekInfoId Id { get; set; }
    public HouseId HouseId { get; set; }
    public WeekStatus Status { get; set; }
    public bool OnTime { get; set; }
}