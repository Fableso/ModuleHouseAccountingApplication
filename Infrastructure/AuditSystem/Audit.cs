namespace Infrastructure.AuditSystem;

public class Audit 
{ 
    public Guid Id { get; set; }
    public string Operation { get; set; } = string.Empty;
    public string TableName { get; set; } = string.Empty;
    public string RecordId { get; set; } = string.Empty;
    public IEnumerable<AuditEntry> Changes { get; set; } = null!;
    public DateTime? ChangeDate { get; set; }
}