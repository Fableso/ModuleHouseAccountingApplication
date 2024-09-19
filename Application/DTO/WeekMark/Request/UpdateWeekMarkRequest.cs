using Domain.Enums;
using Domain.StronglyTypedIds;

namespace Application.DTO.WeekMark.Request;

public sealed class UpdateWeekMarkRequest : IWeekMarkRequest
{
    public WeekMarkId Id { get; set; }
    
    public MarkType MarkType { get; set; }

    public string? Comment { get; set; }
}