using Domain.Common;
using Domain.Enums;
using Domain.StronglyTypedIds;
using Domain.ValueObjects;

namespace Domain.Entities;

public class House : BaseEntity<HouseId>
{
    private House()
    {
        _housePosts = new List<HousePost>();
        _houseWeekInfos = new List<HouseWeekInfo>();
    }
    public House(HouseId houseId, HouseMetrics houseMetrics, Point topLeftCornerCoordinates, HouseStatus currentState,
        DateSpan termsAccordingToDocuments, Brigade brigade, DateSpan? realTerms = null) : this()
    {
        Id = houseId;
        Length = houseMetrics.Length;
        Width = houseMetrics.Width;
        TopLeftCornerX = topLeftCornerCoordinates.X;
        TopLeftCornerY = topLeftCornerCoordinates.Y;
        CurrentState = currentState;
        OfficialStartDate = termsAccordingToDocuments.StartDate;
        OfficialEndDate = termsAccordingToDocuments.EndDate;
        RealStartDate = realTerms?.StartDate;
        RealEndDate = realTerms?.EndDate;
        Brigade = brigade.Value;
    }
    
    public double Length { get; private set; }
    public double Width { get; private set; }
    
    public int TopLeftCornerX { get; private set; }
    
    public int TopLeftCornerY { get;  private set; }
    
    public HouseStatus CurrentState { get;  set; }
    
    public DateOnly OfficialStartDate { get; private set; }
    
    public DateOnly? OfficialEndDate { get; private set; }

    public DateOnly? RealStartDate { get; private set; }

    public DateOnly? RealEndDate { get; private set; }

    public string Brigade { get; private set; } = string.Empty;
    
    public IReadOnlyList<HousePost> HousePosts => _housePosts;
    public IReadOnlyList<HouseWeekInfo> HouseWeekInfos => _houseWeekInfos;

    private List<HousePost> _housePosts;
    private List<HouseWeekInfo> _houseWeekInfos;
    
    public void ChangeBrigade(Brigade newBrigade)
    {
        Brigade = newBrigade.Value;
    }
    
    public void ChangeHousePosition(Point newTopLeftCornerCoordinates)
    {
        TopLeftCornerX = newTopLeftCornerCoordinates.X;
        TopLeftCornerY = newTopLeftCornerCoordinates.Y;
    }
    
    public void ChangeHouseMetrics(HouseMetrics newHouseMetrics)
    {
        Length = newHouseMetrics.Length;
        Width = newHouseMetrics.Width;
    }
    
    public void ChangeDocumentsTerms(DateSpan newTerms)
    {
        OfficialStartDate = newTerms.StartDate;
        OfficialEndDate = newTerms.EndDate;
    }
    
    public void ChangeRealTerms(DateSpan newRealTerms)
    {
        RealStartDate = newRealTerms.StartDate;
        RealEndDate = newRealTerms.EndDate;
    }
}