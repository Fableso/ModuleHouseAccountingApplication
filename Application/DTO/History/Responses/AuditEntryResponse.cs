namespace Application.DTO.History.Responses;

/// <summary>
/// Represents an audit log entry for a specific change made to an entity in the system.
/// </summary>
/// <remarks>
/// The <see cref="AuditEntryResponse"/> class is used to provide detailed information about modifications to a specific field of an entity.
/// It includes the name of the field that was changed, the previous value, and the new value.
/// It can represent entity creations, updates, or deletions.
/// </remarks>
public class AuditEntryResponse
{
    /// <summary>
    /// Gets or sets the name of the field that was changed.
    /// </summary>
    /// <example>Length</example>
    public string? FieldName { get; set; }

    /// <summary>
    /// Gets or sets the old value of the field before the change was made.
    /// This value is null for newly created fields.
    /// </summary>
    /// <example>56.3</example>
    public string? OldValue { get; set; }

    /// <summary>
    /// Gets or sets the new value of the field after the change.
    /// This value is null if the entity is being deleted.
    /// </summary>
    /// <example>67.8</example>
    public string? NewValue { get; set; }
}
