using Domain.StronglyTypedIds;

namespace Application.DTO.House.Request;

public sealed class CreateHouseRequest
{
    public HouseId Id { get; set; }
    public double Length { get; set; }
    public double Width { get; set; }
    public int TopLeftCornerX { get; set; }
    public int TopLeftCornerY { get; set; }
    public DateOnly OfficialStartDate { get; set; }
    public DateOnly? OfficialEndDate { get; set; }
    public string Brigade { get; set; } = string.Empty;
    public List<PostId> PostIds { get; set; } = [];
}