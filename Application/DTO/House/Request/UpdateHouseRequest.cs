using Domain.Enums;
using Domain.StronglyTypedIds;

namespace Application.DTO.House.Request;

/// <summary>
/// Represents a request to update an existing house in the system.
/// </summary>
/// <remarks>
/// The <see cref="UpdateHouseRequest"/> is used to update the information about an existing house entity, 
/// including details such as dimensions, location, current status, start and end dates, and related posts.
/// </remarks>
public sealed class UpdateHouseRequest : IHouseRequest
{
    /// <summary>
    /// Gets or sets the identifier of the house model to update.
    /// </summary>
    /// <example>TestHouseToUpdate</example>
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
    /// <example>120</example>
    public int TopLeftCornerX { get; set; }

    /// <summary>
    /// Gets or sets the Y coordinate of the top-left corner of the house's position on the plot.
    /// </summary>
    /// <example>240</example>
    public int TopLeftCornerY { get; set; }

    /// <summary>
    /// Gets or sets the official planned start date for the house construction.
    /// </summary>
    /// <example>2024-02-01</example>
    public DateOnly OfficialStartDate { get; set; }

    /// <summary>
    /// Gets or sets the official planned end date for the house construction.
    /// This value may be null if the project has no set completion date.
    /// </summary>
    /// <example>2024-11-30</example>
    public DateOnly? OfficialEndDate { get; set; }

    /// <summary>
    /// Gets or sets the name of the brigade responsible for constructing the house.
    /// </summary>
    /// <example>Beta Brigade</example>
    public string Brigade { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the current status of the house.
    /// </summary>
    /// <example>HasTechnicalAssigment</example>
    public HouseStatus CurrentState { get; set; }

    /// <summary>
    /// Gets or sets the actual start date of the house construction.
    /// </summary>
    /// <example>2024-02-10</example>
    public DateOnly RealStartDate { get; set; }

    /// <summary>
    /// Gets or sets the actual end date of the house construction.
    /// This value can be null if the house is still under construction or the end date is unknown.
    /// </summary>
    /// <example>2024-11-15</example>
    public DateOnly? RealEndDate { get; set; }

    /// <summary>
    /// Gets or sets the list of post identifiers associated with the house.
    /// </summary>
    /// <example>
    /// [2, 9, 1]
    /// </example>
    public List<PostId> PostIds { get; set; } = [];
}
