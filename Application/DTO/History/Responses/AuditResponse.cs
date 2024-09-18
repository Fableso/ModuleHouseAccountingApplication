namespace Application.DTO.History.Responses;

public class AuditResponse
{
    public string Operation { get; set; } = string.Empty;
    public string TableName { get; set; } = string.Empty;
    public string RecordId { get; set; } = string.Empty;
    public IEnumerable<AuditEntryResponse> Changes { get; set; } = null!;
    public DateTime? ChangeDate { get; set; }
}