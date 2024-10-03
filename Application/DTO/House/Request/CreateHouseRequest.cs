using Domain.StronglyTypedIds;

namespace Application.DTO.House.Request;

/// <summary>
/// Represents a request to create a new house in the system.
/// </summary>
/// <remarks>
/// The <see cref="CreateHouseRequest"/> is used to provide the necessary information to create a house entity, including its size, location,
/// associated brigade, and other details.
/// </remarks>
public sealed class CreateHouseRequest : IHouseRequest
{
    /// <summary>
    /// Gets or sets the identifier of the house model.
    /// </summary>
    /// <example>TestHouse</example>
    public HouseId Model { get; set; }

    /// <summary>
    /// Gets or sets the length of the house in meters.
    /// </summary>
    /// <example>25.0</example>
    public double Length { get; set; }

    /// <summary>
    /// Gets or sets the width of the house in meters.
    /// </summary>
    /// <example>15.0</example>
    public double Width { get; set; }

    /// <summary>
    /// Gets or sets the X coordinate of the top-left corner of the house's position on the plot.
    /// </summary>
    /// <example>100</example>
    public int TopLeftCornerX { get; set; }

    /// <summary>
    /// Gets or sets the Y coordinate of the top-left corner of the house's position on the plot.
    /// </summary>
    /// <example>200</example>
    public int TopLeftCornerY { get; set; }

    /// <summary>
    /// Gets or sets the official start date for the house construction.
    /// </summary>
    /// <example>2024-01-15</example>
    public DateOnly OfficialStartDate { get; set; }

    /// <summary>
    /// Gets or sets the official end date for the house construction.
    /// This value can be null if the end date is not yet determined.
    /// </summary>
    /// <example>2024-12-15</example>
    public DateOnly? OfficialEndDate { get; set; }

    /// <summary>
    /// Gets or sets the name of the brigade responsible for constructing the house.
    /// </summary>
    /// <example>Alpha Brigade</example>
    public string Brigade { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of post identifiers associated with the house.
    /// </summary>
    /// <example>
    /// [1, 3, 6]
    /// </example>
    public List<PostId> PostIds { get; set; } = [];
}
