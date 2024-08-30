using Domain.Common;
using Domain.Enums;
using Domain.StronglyTypedIds;
using Domain.ValueObjects;

namespace Domain.Entities;

public class WeekMark : BaseEntity<WeekMarkId>
{
    private WeekMark() { }

    public WeekMark(HouseWeekInfoId houseWeekInfoId, MarkType markType, MarkComment comment)
    {
        HouseWeekInfoId = houseWeekInfoId;
        MarkType = markType;
        Comment = comment.Value;
    }
    public HouseWeekInfoId HouseWeekInfoId { get; private set; }
    
    public MarkType MarkType { get; set; }

    public string Comment { get; set; } = string.Empty;
    
    public HouseWeekInfo HouseWeekInfo { get; private set; }

    public WeekMark UpdateWeekMark(WeekMark weekMark)
    {
        MarkType = weekMark.MarkType;
        Comment = weekMark.Comment;
        return this;
    }
}