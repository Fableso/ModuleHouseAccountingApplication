using Application.DTO.WeekMark.Response;
using Domain.Enums;
using Domain.StronglyTypedIds;

namespace Application.DTO.HouseWeekInfo.Response;

/// <summary>
/// Represents the response data for a specific weekly progress entry related to a house.
/// </summary>
/// <remarks>
/// The <see cref="HouseWeekInfoResponse"/> contains detailed information about the progress made during a specific week
/// in the construction of a house. This includes the start date, status, whether the work was on time, and any marks 
/// or comments that were added during that week to provide additional insights.
/// </remarks>
public class HouseWeekInfoResponse
{
    /// <summary>
    /// Gets or sets the identifier for the weekly progress entry.
    /// </summary>
    /// <example>1</example>
    public HouseWeekInfoId Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the house model to which this weekly progress entry belongs.
    /// </summary>
    /// <example>"HouseModel_123"</example>
    public HouseId HouseModel { get; set; }

    /// <summary>
    /// Gets or sets the start date of the week's progress.
    /// </summary>
    /// <example>2023-03-01</example>
    public DateOnly StartDate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the progress during this week was completed on time.
    /// </summary>
    /// <example>true</example>
    public bool OnTime { get; set; }

    /// <summary>
    /// Gets or sets the current status of the weekly progress.
    /// </summary>
    /// <example>"InProcess"</example>
    public WeekStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the list of marks associated with the weekly progress.
    /// Each mark provides information about specific notes, comments, or issues observed during that week.
    /// </summary>
    /// <example>
    /// [
    ///   {
    ///     "id": "e90cd1ae331b487f94cdae78923e3851",
    ///     "weekInfoId": 1,
    ///     "markType": "BlueMark",
    ///     "comment": "The roof requires quality check.",
    ///   },
    ///   {
    ///     "id": "17cb6b74d9a847b0ad6866693ea362f0",
    ///     "weekInfoId": 1,
    ///     "markType": "RedMark",
    ///     "comment": "The foundation is not stable.",
    ///   }
    /// ]
    /// </example>
    public List<WeekMarkResponse> WeekMarkResponses { get; set; } = [];
}
