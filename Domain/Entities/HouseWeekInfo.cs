using Domain.Common;
using Domain.Enums;
using Domain.StronglyTypedIds;
using Domain.ValueObjects;
using FluentResults;

namespace Domain.Entities;

public class HouseWeekInfo : BaseEntity<HouseWeekInfoId>
{
    private HouseWeekInfo()
    {
        _weekComments = new List<WeekMark>();
    }

    public HouseWeekInfo(HouseId houseId, WeekStartDate startDate, WeekStatus status, bool onTime) : this()
    {
        HouseId = houseId;
        StartDate = startDate.Value;
        Status = status;
        OnTime = onTime;
    }
    
    public HouseId HouseId { get; private set; }
    public DateOnly StartDate { get; private set; }
    public WeekStatus Status { get; private set; }
    public bool OnTime { get; private set; }

    public House House { get; private set; } = null!;
    
    public IReadOnlyList<WeekMark> WeekComments => _weekComments;
    private List<WeekMark> _weekComments;
}