using Domain;
using Domain.Entities;
using Domain.StronglyTypedIds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public sealed class WeekMarkConfiguration : IEntityTypeConfiguration<WeekMark>
{
    public void Configure(EntityTypeBuilder<WeekMark> builder)
    {
        builder.Property(wm => wm.Id)
            .HasConversion(id => id.Value, value => new WeekMarkId(value))
            .ValueGeneratedOnAdd();

        builder.Property(wm => wm.HouseWeekInfoId)
            .HasConversion(id => id.Value, value => new HouseWeekInfoId(value));
        
        builder.Property(c => c.Comment).HasMaxLength(DomainConstants.MaxWeekMarkCommentLength);
    }
}