using Microsoft.EntityFrameworkCore;

namespace Infrastructure.AuditSystem;

public class EntityAuditInformation
{
    public dynamic Entity { get; set; } = null!;
    public string TableName { get; set; } = string.Empty;

    public EntityState State { get; set; }

    public List<AuditEntry> Changes { get; set; } = [];
    
    public string OperationType
    {
        get
        {
            switch (State)
            {
                case EntityState.Added:
                    return "Create";
                case EntityState.Modified:
                   return "Update";
                case EntityState.Deleted:
                    return "Delete";
                case EntityState.Detached:
                case EntityState.Unchanged:
                default:
                    return string.Empty;
            }
        }
    }

}