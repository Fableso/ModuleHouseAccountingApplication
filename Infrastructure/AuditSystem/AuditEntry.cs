namespace Infrastructure.AuditSystem;

public class AuditEntry
{
    public Guid Id { get; set; }
    public string? FieldName { get; set; }

    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
    
    public Guid AuditId { get; set; }
    public Audit Audit { get; set; } = null!;
}