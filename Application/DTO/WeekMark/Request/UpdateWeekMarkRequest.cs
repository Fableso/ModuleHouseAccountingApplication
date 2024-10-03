using Domain.Enums;
using Domain.StronglyTypedIds;

namespace Application.DTO.WeekMark.Request;

/// <summary>
/// Represents the request data to update an existing mark for a specific week in the house construction timeline.
/// </summary>
/// <remarks>
/// The <see cref="UpdateWeekMarkRequest"/> is used to modify details related to an existing mark that has been previously added
/// to a specific week's progress in a house's construction. Marks can be used to indicate quality issues, notes, or other relevant observations.
/// </remarks>
public sealed class UpdateWeekMarkRequest : IWeekMarkRequest
{
    /// <summary>
    /// Gets or sets the identifier of the existing week mark that needs to be updated.
    /// </summary>
    /// <remarks>
    /// The identifier is used to uniquely locate the specific mark that needs to be modified.
    /// </remarks>
    /// <example>e9817b01237641d19c3c964f340bb1b4</example>
    public WeekMarkId Id { get; set; }

    /// <summary>
    /// Gets or sets the updated type of mark for the weekly progress.
    /// </summary>
    /// <remarks>
    /// The <see cref="MarkType"/> defines the category or classification of the mark, such as "RedMark", "BlueMark", etc.
    /// It helps in identifying whether the note is a warning, a quality assessment, or another type of observation.
    /// </remarks>
    /// <example>BlueMark</example>
    public MarkType MarkType { get; set; }

    /// <summary>
    /// Gets or sets an optional comment providing additional information about the updated mark.
    /// </summary>
    /// <remarks>
    /// The comment provides context or details regarding the mark, such as a note about a detected issue or a recommendation.
    /// This property can be null if no comment is necessary or if the previous comment remains unchanged.
    /// </remarks>
    /// <example>Roof structure has been rechecked and now meets quality standards.</example>
    public string? Comment { get; set; }
}
