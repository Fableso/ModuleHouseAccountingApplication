namespace Application.DTO.History.Responses;

public class AuditEntryResponse
{
    public string? FieldName { get; set; }

    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
}