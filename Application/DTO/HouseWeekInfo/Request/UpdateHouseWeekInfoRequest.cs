using Domain.Enums;
using Domain.StronglyTypedIds;

namespace Application.DTO.HouseWeekInfo.Request;

/// <summary>
/// Represents the request data to update an existing weekly progress entry for a house.
/// </summary>
/// <remarks>
/// The <see cref="UpdateHouseWeekInfoRequest"/> is used to modify the information related to a specific week's progress
/// for a house's construction. It includes details like the identifier of the weekly progress, the current status of the week, 
/// and whether the work was completed on time.
/// </remarks>
public sealed class UpdateHouseWeekInfoRequest : IHouseWeekInfoRequest
{
    /// <summary>
    /// Gets or sets the identifier of the house weekly progress entry that needs to be updated.
    /// </summary>
    /// <example>1</example>
    public HouseWeekInfoId Id { get; set; }

    /// <summary>
    /// Gets or sets the updated status of the weekly progress.
    /// </summary>
    /// <example>"OnHold"</example>
    public WeekStatus Status { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the work for this week was completed on time.
    /// </summary>
    /// <example>false</example>
    public bool OnTime { get; set; }
}
