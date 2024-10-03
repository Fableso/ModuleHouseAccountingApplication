using Domain.Enums;
using Domain.StronglyTypedIds;

namespace Application.DTO.WeekMark.Response;

/// <summary>
/// Represents the response data for a mark associated with a specific week's progress in house construction.
/// </summary>
/// <remarks>
/// The <see cref="WeekMarkResponse"/> provides detailed information about a mark that has been added to a specific week's construction progress.
/// It includes identifiers, the type of the mark, and any additional comments associated with that mark.
/// </remarks>
public class WeekMarkResponse
{
    /// <summary>
    /// Gets or sets the unique identifier for the week mark.
    /// </summary>
    /// <remarks>
    /// The identifier is used to uniquely identify this particular mark within the system.
    /// </remarks>
    /// <example>e9817b01237641d19c3c964f340bb1b4</example>
    public WeekMarkId Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the house weekly progress entry to which this mark is related.
    /// </summary>
    /// <remarks>
    /// This property represents the specific week of house construction that the mark pertains to.
    /// </remarks>
    /// <example>1</example>
    public HouseWeekInfoId HouseWeekInfoId { get; set; }

    /// <summary>
    /// Gets or sets the type of mark associated with the weekly progress.
    /// </summary>
    /// <remarks>
    /// The <see cref="MarkType"/> categorizes the type of mark, such as "RedMark" to indicate an issue or "BlueMark" for observations or recommendations.
    /// This helps in identifying the nature of the comment or note.
    /// </remarks>
    /// <example>"RedMark"</example>
    public MarkType MarkType { get; set; }

    /// <summary>
    /// Gets or sets an optional comment providing further details about the mark.
    /// </summary>
    /// <remarks>
    /// The comment provides additional context or notes regarding the mark, such as identified issues or suggestions for improvement.
    /// This property may be null if no specific comment was added.
    /// </remarks>
    /// <example>"The roof requires a quality check due to observed sagging."</example>
    public string? Comment { get; set; }
}
