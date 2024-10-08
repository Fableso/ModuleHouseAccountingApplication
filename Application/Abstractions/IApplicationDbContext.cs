using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Abstractions;

public interface IApplicationDbContext
{
    DbSet<House> Houses { get; }
    
    DbSet<HousePost> HousePosts { get; }
    DbSet<Post> Posts { get; }
    DbSet<HouseWeekInfo> HouseWeekInfos { get; }
    DbSet<WeekMark> WeekMarks { get; }
    DbSet<Audit> Audits { get; }
    DbSet<AuditEntry> AuditEntries { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}