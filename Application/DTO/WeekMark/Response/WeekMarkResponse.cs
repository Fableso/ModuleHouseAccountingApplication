using Domain.Enums;
using Domain.StronglyTypedIds;

namespace Application.DTO.WeekMark.Response;

public class WeekMarkResponse
{
    public WeekMarkId Id { get; set; }
    public HouseWeekInfoId HouseWeekInfoId { get; set; }
    
    public MarkType MarkType { get; set; }

    public string? Comment { get; set; }
}