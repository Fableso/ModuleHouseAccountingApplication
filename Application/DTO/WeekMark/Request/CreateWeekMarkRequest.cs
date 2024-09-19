using Domain.Enums;
using Domain.StronglyTypedIds;

namespace Application.DTO.WeekMark.Request;

public sealed class CreateWeekMarkRequest : IWeekMarkRequest
{
    public HouseWeekInfoId HouseWeekInfoId { get; set; }
    
    public MarkType MarkType { get; set; }

    public string? Comment { get; set; }
}