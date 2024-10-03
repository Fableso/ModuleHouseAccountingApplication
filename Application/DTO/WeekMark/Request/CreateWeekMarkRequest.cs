using Domain.Enums;
using Domain.StronglyTypedIds;

namespace Application.DTO.WeekMark.Request;

/// <summary>
/// Represents the request data to create a new mark for a specific week in the house construction timeline.
/// </summary>
/// <remarks>
/// The <see cref="CreateWeekMarkRequest"/> is used to add marks, comments, or notes to a specific week's progress of a house's construction.
/// Marks can indicate quality assessments, issues, or any other notes that need to be tracked for that particular week.
/// </remarks>
public sealed class CreateWeekMarkRequest : IWeekMarkRequest
{
    /// <summary>
    /// Gets or sets the identifier of the house weekly progress entry to which this mark is related.
    /// </summary>
    /// <remarks>
    /// This identifier uniquely locates the specific week's progress to which the mark will be added.
    /// </remarks>
    /// <example>1</example>
    public HouseWeekInfoId HouseWeekInfoId { get; set; }

    /// <summary>
    /// Gets or sets the type of mark being added to the weekly progress.
    /// </summary>
    /// <remarks>
    /// The <see cref="MarkType"/> indicates the category of the mark, such as a quality assessment or an identified issue.
    /// Typical values could include "RedMark", "BlueMark", etc., to signify different types of notes or flags.
    /// </remarks>
    /// <example>RedMark</example>
    public MarkType MarkType { get; set; }

    /// <summary>
    /// Gets or sets an optional comment providing additional information about the mark.
    /// </summary>
    /// <remarks>
    /// The comment can include any relevant details regarding the mark, such as a note on an issue or a recommendation.
    /// This value is optional and can be null if no specific comment is needed.
    /// </remarks>
    /// <example>The foundation requires additional inspection due to visible cracks.</example>
    public string? Comment { get; set; }
}
