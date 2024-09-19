using Domain.Enums;

namespace Application.DTO.WeekMark.Request;

public interface IWeekMarkRequest
{
    public MarkType MarkType { get; set; }

    public string? Comment { get; set; }
}