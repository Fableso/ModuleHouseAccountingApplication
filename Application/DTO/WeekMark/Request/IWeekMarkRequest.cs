using Domain.Enums;

namespace Application.DTO.WeekMark.Request;

/// <summary>
/// Defines the structure required for requests related to adding or updating marks for a specific week of house construction.
/// </summary>
/// <remarks>
/// The <see cref="IWeekMarkRequest"/> interface provides the common properties that are needed to handle marks associated with a specific week's progress.
/// This ensures a consistent format for any requests involving creating or updating marks, such as observations or quality control notes.
/// </remarks>
public interface IWeekMarkRequest
{
    /// <summary>
    /// Gets or sets the type of mark being associated with the weekly progress.
    /// </summary>
    /// <remarks>
    /// The <see cref="MarkType"/> defines the classification of the mark, such as a "RedMark" to indicate an issue or a "BlueMark" for general notes.
    /// This allows for effective categorization of observations during construction.
    /// </remarks>
    /// <example>RedMark</example>
    MarkType MarkType { get; set; }

    /// <summary>
    /// Gets or sets an optional comment that provides additional information about the mark.
    /// </summary>
    /// <remarks>
    /// The comment is used to describe any relevant information related to the mark. It is optional and may be left empty if no comment is necessary.
    /// Examples might include notes on quality issues, suggestions, or any specific observations.
    /// </remarks>
    /// <example>Inspect the foundation cracks.</example>
    string? Comment { get; set; }
}
