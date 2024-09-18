namespace Domain.Entities;

public class AuditEntry
{
    public Guid Id { get; init; }
    public string? FieldName { get; init; }

    public string? OldValue { get; init; }
    public string? NewValue { get; init; }
    
    public Guid AuditId { get; init; }
    public Audit Audit { get; private set; } = null!;
}