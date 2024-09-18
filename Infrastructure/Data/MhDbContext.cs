using System.Reflection;
using Application.Abstractions;
using Domain.Entities;
using Infrastructure.AuditSystem;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Data;

public class MhDbContext : DbContext, IApplicationDbContext
{
    public MhDbContext(DbContextOptions<MhDbContext> options)
        : base(options)
    {
    }

    public DbSet<House> Houses => Set<House>();
    public DbSet<HousePost> HousePosts => Set<HousePost>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<HouseWeekInfo> HouseWeekInfos => Set<HouseWeekInfo>();
    public DbSet<WeekMark> WeekMarks => Set<WeekMark>();
    public DbSet<AuditEntry> AuditEntries => Set<AuditEntry>();
    public DbSet<Audit> Audits => Set<Audit>();

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        await using var transaction = await Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var entityAuditInformation = TrackChanges();
            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

            await AddAuditAsync(entityAuditInformation, cancellationToken);

            await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            
            await transaction.CommitAsync(cancellationToken);

            return result;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private List<EntityAuditInformation> TrackChanges()
    {
        var entityEntries = ChangeTracker.Entries()
            .Where(x => x.State != EntityState.Unchanged)
            .ToList();

        return entityEntries.Select(CreateAuditInformation).ToList();
    }

    private static EntityAuditInformation CreateAuditInformation(EntityEntry entityEntry)
    {
        dynamic entity = entityEntry.Entity;
        List<AuditEntry>? changes = new();
        if (entityEntry.State == EntityState.Modified)
        {
            changes = GetEntityChanges(entityEntry);
        }
        
        return new EntityAuditInformation
        {
            Entity = entity,
            TableName = entityEntry.Metadata?.GetTableName() ?? entity.GetType().Name,
            State = entityEntry.State,
            Changes = changes 
        };
    }

    private static List<AuditEntry> GetEntityChanges(EntityEntry entityEntry)
    {
        return entityEntry.Properties
            .Where(property => property.IsModified && 
                               property.Metadata.Name != "Id" &&
                               !Equals(property.CurrentValue, property.OriginalValue))
            .Select(property => new AuditEntry
            {
                FieldName = property.Metadata.Name,
                NewValue = property.CurrentValue?.ToString(),
                OldValue = property.OriginalValue?.ToString()
            })
            .ToList();
    }
    public async Task AddAuditAsync(List<EntityAuditInformation> auditInformation, CancellationToken cancellationToken)
    {
        foreach (var item in auditInformation)
        {
            Audit audit = new()
            {
                TableName = item.TableName,
                RecordId = item.Entity.Id.ToString(),
                ChangeDate = DateTime.Now,
                Operation = item.OperationType,
                Changes = item.Changes,
            };
            await Audits.AddAsync(audit, cancellationToken);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
