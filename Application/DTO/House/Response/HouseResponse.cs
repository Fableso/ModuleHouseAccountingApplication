using Application.DTO.HouseWeekInfo.Response;
using Application.DTO.Post.Response;
using Domain.Enums;
using Domain.StronglyTypedIds;

namespace Application.DTO.House.Response;

public sealed class HouseResponse
{
    public HouseId Model { get; set; }
    
    public double Length { get; set; }
    
    public double Width { get; set; }
    
    public int TopLeftCornerX { get; set; }
    
    public int TopLeftCornerY { get; set; }
    
    public DateOnly OfficialStartDate { get; set; }
    
    public DateOnly? OfficialEndDate { get; set; }
    
    public DateOnly? RealStartDate { get; set; }
    
    public DateOnly? RealEndDate { get; set; }
    
    public HouseStatus CurrentState { get; set; }

    public string Brigade { get; set; } = string.Empty;
    
    public List<PostResponse> Posts { get; set; } = [];
    public List<HouseWeekInfoResponse> HouseWeekInfos { get; set; } = [];
}