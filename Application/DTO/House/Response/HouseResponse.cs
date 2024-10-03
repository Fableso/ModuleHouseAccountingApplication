using Application.DTO.HouseWeekInfo.Response;
using Application.DTO.Post.Response;
using Domain.Enums;
using Domain.StronglyTypedIds;

namespace Application.DTO.House.Response;

/// <summary>
/// Represents the response data for an existing house entity in the system.
/// </summary>
/// <remarks>
/// The <see cref="HouseResponse"/> contains all necessary information about a house entity,
/// including its dimensions, coordinates, construction timeline, status, the assigned brigade,
/// and related information about posts and weekly progress.
/// This DTO is intended to be used as the response for endpoints that query house details.
/// </remarks>
public sealed class HouseResponse
{
    /// <summary>
    /// Gets or sets the identifier or name of the house model.
    /// </summary>
    /// <example>"HouseModel_123"</example>
    public HouseId Model { get; set; }

    /// <summary>
    /// Gets or sets the length of the house in meters.
    /// </summary>
    /// <example>30.0</example>
    public double Length { get; set; }

    /// <summary>
    /// Gets or sets the width of the house in meters.
    /// </summary>
    /// <example>20.0</example>
    public double Width { get; set; }

    /// <summary>
    /// Gets or sets the X coordinate of the top-left corner of the house's position on the plot.
    /// </summary>
    /// <example>-10</example>
    public int TopLeftCornerX { get; set; }

    /// <summary>
    /// Gets or sets the Y coordinate of the top-left corner of the house's position on the plot.
    /// </summary>
    /// <example>15</example>
    public int TopLeftCornerY { get; set; }

    /// <summary>
    /// Gets or sets the official planned start date for the house construction.
    /// </summary>
    /// <example>2023-02-17</example>
    public DateOnly OfficialStartDate { get; set; }

    /// <summary>
    /// Gets or sets the official planned end date for the house construction.
    /// This value can be null if the completion date is not yet determined.
    /// </summary>
    /// <example>2023-06-01</example>
    public DateOnly? OfficialEndDate { get; set; }

    /// <summary>
    /// Gets or sets the actual start date of the house construction.
    /// This value can be null if construction has not yet started.
    /// </summary>
    /// <example>2023-03-01</example>
    public DateOnly? RealStartDate { get; set; }

    /// <summary>
    /// Gets or sets the actual end date of the house construction.
    /// This value can be null if construction has not yet been completed.
    /// </summary>
    /// <example>2023-12-15</example>
    public DateOnly? RealEndDate { get; set; }

    /// <summary>
    /// Gets or sets the current status of the house construction.
    /// </summary>
    /// <example>"Completed"</example>
    public HouseStatus CurrentState { get; set; }

    /// <summary>
    /// Gets or sets the name of the brigade responsible for constructing the house.
    /// </summary>
    /// <example>"Delta Brigade"</example>
    public string Brigade { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of post details associated with the house.
    /// Each post contains information related to specific tasks or components involved in the house construction.
    /// </summary>
    /// <example>
    /// [
    ///   {
    ///     "id": 1,
    ///     "name": "Main",
    ///     "Area": 70
    ///   },
    ///   {
    ///     "id": "4",
    ///     "name": "Backyard",
    ///     "Area": 70
    ///   }
    /// ]
    /// </example>
    public List<PostResponse> Posts { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of weekly progress details related to the house.
    /// Each entry provides information on what was done during a specific week.
    /// </summary>
    /// <example>
    /// [
    ///   {
    ///     "id": 1,
    ///     "houseModel": "HouseModel_123",
    ///     "startDate": "2023-03-01",
    ///     "onTime": true,
    ///     "status": "InProcess"
    ///     "weekMarkResponses": [
    ///         {
    ///             "id": e90cd1ae331b487f94cdae78923e3851,
    ///             "weekInfoId": 1,
    ///             "MarkType": "BlueMark",
    ///             "Comment": "The roof requires quality check.",
    ///         },
    ///         {
    ///             "id": 17cb6b74d9a847b0ad6866693ea362f0,
    ///             "weekInfoId": 1,
    ///             "MarkType": "RedMark",
    ///             "Comment": "The foundation is not stable.",
    ///         }
    ///     ]
    ///   },
    ///    {
    ///     "id": 2,
    ///     "houseModel": "HouseModel_123",
    ///     "startDate": "2023-05-18",
    ///     "onTime": false,
    ///     "status": "OnHold"
    ///     "weekMarkResponses": [
    ///         {
    ///             "id": a5a469b7c00a41308258dd11e8ef21c1,
    ///             "weekInfoId": 2,
    ///             "MarkType": "GreenMark",
    ///             "Comment": "The walls are painted.",
    ///         }
    ///     ]
    ///    }
    /// ]
    /// </example>
    public List<HouseWeekInfoResponse> HouseWeekInfos { get; set; } = [];
}
