namespace Domain.Entities;

public class Audit 
{ 
    public Guid Id { get; init; }
    public string Operation { get; init; } = string.Empty;
    public string TableName { get; init; } = string.Empty;
    public string RecordId { get; init; } = string.Empty;
    public IEnumerable<AuditEntry> Changes { get; init; } = null!;
    public DateTime? ChangeDate { get; init; }
}