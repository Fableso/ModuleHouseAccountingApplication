using Application.Abstractions;
using Domain.Enums;
using Domain.StronglyTypedIds;

namespace Application.DTO.HouseWeekInfo.Request;

public sealed class CreateHouseWeekInfoRequest : IHouseWeekInfoRequest
{
    public HouseId HouseId { get; set; }
    public DateOnly StartDate { get; set; }
    public bool OnTime { get; set; }
    public WeekStatus Status { get; set; }
    
}