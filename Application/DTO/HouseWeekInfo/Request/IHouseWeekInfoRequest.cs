using Domain.Enums;
using Domain.StronglyTypedIds;

namespace Application.DTO.HouseWeekInfo.Request;

/// <summary>
/// Defines the structure for a weekly progress entry related to house construction.
/// </summary>
/// <remarks>
/// The <see cref="IHouseWeekInfoRequest"/> interface provides the common properties required for handling weekly progress
/// updates or creation requests related to a house's construction process. This interface ensures consistency 
/// across different types of week-related requests by defining essential properties such as the status and whether the progress was on time.
/// </remarks>
public interface IHouseWeekInfoRequest
{
    /// <summary>
    /// Gets or sets the status of the weekly progress.
    /// </summary>
    /// <example>"InProcess"</example>
    WeekStatus Status { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the work for this week was completed on time.
    /// </summary>
    /// <example>true</example>
    bool OnTime { get; set; }
}
