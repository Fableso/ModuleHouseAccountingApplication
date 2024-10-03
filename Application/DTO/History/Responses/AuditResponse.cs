namespace Application.DTO.History.Responses;

/// <summary>
/// Represents an audit record containing details of an operation performed on an entity in the system.
/// </summary>
/// <remarks>
/// The <see cref="AuditResponse"/> class provides detailed information regarding an operation (such as Create, Update, or Delete)
/// that was performed on an entity. This includes information about the table affected, the record's identifier, 
/// the specific changes made, the date of the change, and the author responsible for the change.
/// </remarks>
public class AuditResponse
{
    /// <summary>
    /// Gets or sets the type of operation performed.
    /// This could be operations such as "CREATE", "UPDATE", or "DELETE".
    /// </summary>
    /// <example>UPDATE</example>
    public string Operation { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the table in the database where the change occurred.
    /// </summary>
    /// <example>Users</example>
    public string TableName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the identifier of the record that was changed.
    /// This identifier could be a primary key or a unique identifier for the affected record.
    /// </summary>
    /// <example>12345</example>
    public string RecordId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a collection of changes made to the entity's fields.
    /// Each change is represented by an <see cref="AuditEntryResponse"/> object.
    /// </summary>
    /// <example>
    /// {
    ///     new AuditEntryResponse { FieldName = "Username", OldValue = "JohnDoe", NewValue = "JohnSmith" },
    ///     new AuditEntryResponse { FieldName = "Email", OldValue = "john.doe@example.com", NewValue = "john.smith@example.com" }
    /// };
    /// </example>
    public IEnumerable<AuditEntryResponse> Changes { get; set; } = null!;

    /// <summary>
    /// Gets or sets the date and time when the change was made.
    /// This can be null if the change date is not available.
    /// </summary>
    /// <example>2024-10-05T12:45:00Z</example>
    public DateTime? ChangeDate { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who made the change.
    /// This could be the user ID or another unique identifier.
    /// It is nullable to handle cases where the author is unknown or automated.
    /// </summary>
    /// <example>user_98765</example>
    public string? ChangeAuthorId { get; set; }
}
