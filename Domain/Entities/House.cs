using Domain.Common;
using Domain.Enums;
using Domain.StronglyTypedIds;
using Domain.ValueObjects;

namespace Domain.Entities;

public class House : BaseEntity<HouseId>
{
    private House()
    {
        _posts = new List<Post>();
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
    
    public HouseStatus CurrentState { get;  private set; }
    
    public DateOnly OfficialStartDate { get; private set; }
    
    public DateOnly? OfficialEndDate { get; private set; }

    public DateOnly? RealStartDate { get; private set; }

    public DateOnly? RealEndDate { get; private set; }

    public string Brigade { get; private set; } = string.Empty;
    
    public IReadOnlyList<Post> Posts => _posts;

    private List<Post> _posts;
}