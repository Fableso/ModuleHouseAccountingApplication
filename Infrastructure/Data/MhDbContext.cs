using System.Reflection;
using Application.Abstractions;
using Domain.Entities;
using Infrastructure.AuditSystem;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Data;

public class MhDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    private readonly string? _userId;
    public MhDbContext(DbContextOptions<MhDbContext> options, IHttpContextAccessor httpContextAccessor)
        : base(options)
    {
        string? authorizationHeader = httpContextAccessor.HttpContext?.Request.Headers["Authorization"];
        if (!string.IsNullOrEmpty(authorizationHeader) && (authorizationHeader.StartsWith("Bearer") || authorizationHeader.StartsWith("bearer")))
        {
            var user = httpContextAccessor.HttpContext?.User;
            var claim = user?.FindFirst("UserId");
            if (claim is not null)
            {
                _userId = claim.Value;
            }
            else
            {
                throw new UnableToGetChangeAuthorIdException("Unable to determine the change author");
            }
        }
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
        return new EntityAuditInformation
        {
            EntityEntry = entityEntry,
            TableName = entityEntry.Metadata?.GetTableName() ?? entityEntry.Entity.GetType().Name,
            State = entityEntry.State,
            Changes = entityEntry.State == EntityState.Modified ? GetEntityChanges(entityEntry) : new List<AuditEntry>()
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
            var keyName = item.EntityEntry.FindPrimaryKeyPropertyName();

            var keyValue = keyName != null ? item.EntityEntry.Property(keyName).CurrentValue?.ToString() : "UnknownKey";

            Audit audit = new()
            {
                TableName = item.TableName,
                RecordId = keyValue ?? "UnknownKey",
                ChangeDate = DateTime.Now,
                Operation = item.OperationType,
                Changes = item.Changes,
                ChangedById = _userId,
            };

            await Audits.AddAsync(audit, cancellationToken);
        }
    }



    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}

public static class EntityTypeExtensions
{
    public static string? FindPrimaryKeyPropertyName(this EntityEntry entry)
    {
        var key = entry.Metadata.FindPrimaryKey();
        return key?.Properties.FirstOrDefault()?.Name;
    }
}


