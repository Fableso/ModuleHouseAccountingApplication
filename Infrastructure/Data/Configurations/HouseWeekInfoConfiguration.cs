using Domain.Entities;
using Domain.StronglyTypedIds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public sealed class HouseWeekInfoConfiguration : IEntityTypeConfiguration<HouseWeekInfo>
{
    public void Configure(EntityTypeBuilder<HouseWeekInfo> builder)
    {
        builder.Property(hwi => hwi.Id)
            .HasConversion(id => id.Value, value => new HouseWeekInfoId(value))
            .ValueGeneratedOnAdd();

        builder.Property(hwi => hwi.HouseId)
            .HasConversion(id => id.Value, value => new HouseId(value));
    }
}