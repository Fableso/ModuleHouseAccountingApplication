using Domain.StronglyTypedIds;

namespace Application.DTO.House.Request;

/// <summary>
/// Defines the common structure required for house-related requests in the system.
/// </summary>
/// <remarks>
/// The <see cref="IHouseRequest"/> interface provides a template for house creation or update requests,
/// specifying the essential properties needed for the house entity, including its dimensions, position,
/// construction dates, responsible brigade, and associated posts.
/// This interface is intended to be implemented by request DTOs such as <see cref="CreateHouseRequest"/> or <see cref="UpdateHouseRequest"/>.
/// </remarks>
public interface IHouseRequest
{
    /// <summary>
    /// Gets or sets the identifier or name of the house model.
    /// </summary>
    /// <example>TestHouse</example>
    HouseId Model { get; set; }

    /// <summary>
    /// Gets or sets the length of the house in meters.
    /// </summary>
    /// <example>10.0</example>
    double Length { get; set; }

    /// <summary>
    /// Gets or sets the width of the house in meters.
    /// </summary>
    /// <example>8.0</example>
    double Width { get; set; }

    /// <summary>
    /// Gets or sets the X coordinate of the top-left corner of the house's position on the plot.
    /// </summary>
    /// <example>5</example>
    int TopLeftCornerX { get; set; }

    /// <summary>
    /// Gets or sets the Y coordinate of the top-left corner of the house's position on the plot.
    /// </summary>
    /// <example>10</example>
    int TopLeftCornerY { get; set; }

    /// <summary>
    /// Gets or sets the official start date for the house construction.
    /// </summary>
    /// <example>2023-01-01</example>
    DateOnly OfficialStartDate { get; set; }

    /// <summary>
    /// Gets or sets the official end date for the house construction.
    /// This value can be null if the completion date is not yet determined.
    /// </summary>
    /// <example>2023-12-31</example>
    DateOnly? OfficialEndDate { get; set; }

    /// <summary>
    /// Gets or sets the name of the brigade responsible for constructing the house.
    /// </summary>
    /// <example>Main Brigade</example>
    string Brigade { get; set; }

    /// <summary>
    /// Gets or sets the list of post identifiers associated with the house.
    /// </summary>
    /// <example>[1, 2, 3]</example>
    List<PostId> PostIds { get; set; }
}
