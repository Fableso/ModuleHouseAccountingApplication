using Application.DTO.WeekMark.Response;
using Domain.Enums;
using Domain.StronglyTypedIds;

namespace Application.DTO.HouseWeekInfo.Response;

public class HouseWeekInfoResponse
{
    public HouseWeekInfoId Id { get; set; }
    public HouseId HouseId { get; set; } 
    public DateOnly StartDate { get; set; }
    public bool OnTime { get; set; }
    public WeekStatus Status { get; set; }

    public List<WeekMarkResponse> WeekMarkResponses { get; set; } = [];
}