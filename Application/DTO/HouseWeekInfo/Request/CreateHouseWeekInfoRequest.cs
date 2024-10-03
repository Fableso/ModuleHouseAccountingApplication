using Application.Abstractions;
using Domain.Enums;
using Domain.StronglyTypedIds;

namespace Application.DTO.HouseWeekInfo.Request;

/// <summary>
/// Represents the request data to create a new weekly progress entry for a house.
/// </summary>
/// <remarks>
/// The <see cref="CreateHouseWeekInfoRequest"/> is used to specify details for a weekly update
/// on the progress of a house's construction. This includes the house identifier, the start date of the week,
/// whether the work was completed on time, and the current status of the weekly progress.
/// </remarks>
public sealed class CreateHouseWeekInfoRequest : IHouseWeekInfoRequest
{
    /// <summary>
    /// Gets or sets the identifier of the house for which the weekly progress is being created.
    /// </summary>
    /// <example>"House_456"</example>
    public HouseId HouseId { get; set; }

    /// <summary>
    /// Gets or sets the start date of the week's progress.
    /// </summary>
    /// <example>2023-05-01</example>
    public DateOnly StartDate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the progress during this week was on time.
    /// </summary>
    /// <example>true</example>
    public bool OnTime { get; set; }

    /// <summary>
    /// Gets or sets the status of the weekly progress.
    /// </summary>
    /// <example>"InProcess"</example>
    public WeekStatus Status { get; set; }
}
