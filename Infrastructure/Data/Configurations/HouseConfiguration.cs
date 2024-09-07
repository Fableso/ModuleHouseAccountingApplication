using Domain;
using Domain.Entities;
using Domain.StronglyTypedIds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public sealed class HouseConfiguration : IEntityTypeConfiguration<House>
{
    public void Configure(EntityTypeBuilder<House> builder)
    {
        builder.Property(h => h.Id)
            .HasConversion(id => id.Value, value => new HouseId(value));
        
        builder.Property(house => house.TopLeftCornerX)
            .IsRequired();
        builder.Property(house => house.TopLeftCornerY)
            .IsRequired();

        builder.Property(e => e.Brigade)
            .HasMaxLength(DomainConstants.MaxBrigadeNameLength)
            .IsRequired();
        
        builder.Property(m => m.Length).IsRequired();
        builder.Property(m => m.Width).IsRequired();

        builder.Property(d => d.OfficialStartDate);
        builder.Property(d => d.OfficialEndDate);


        builder.Property(d => d.RealStartDate);
        builder.Property(d => d.RealEndDate);
    }
}