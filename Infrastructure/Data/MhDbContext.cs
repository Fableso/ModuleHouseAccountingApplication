using System.Reflection;
using Application.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class MhDbContext : DbContext, IApplicationDbContext
{
    public MhDbContext(DbContextOptions<MhDbContext> options) : base(options) { }

    public DbSet<House> Houses => Set<House>();

    public DbSet<HousePost> HousePosts => Set<HousePost>();

    public DbSet<Post> Posts => Set<Post>();
    
    public DbSet<HouseWeekInfo> HouseWeekInfos => Set<HouseWeekInfo>();

    public DbSet<WeekMark> WeekMarks => Set<WeekMark>();
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}