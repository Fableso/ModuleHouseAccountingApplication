using Domain.Enums;
using Domain.StronglyTypedIds;

namespace Application.DTO.HouseWeekInfo.Request;

public interface IHouseWeekInfoRequest
{
    public WeekStatus Status { get; set; }
    public bool OnTime { get; set; }
}