using Domain.Entities;
using Domain.StronglyTypedIds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public sealed class HousePostConfiguration : IEntityTypeConfiguration<HousePost>
{
    public void Configure(EntityTypeBuilder<HousePost> builder)
    {
        builder.Property(hp => hp.Id)
            .HasConversion(id => id.Value, value => new HousePostId(value))
            .ValueGeneratedOnAdd();

        builder.Property(hp => hp.HouseId)
            .HasConversion(id => id.Value, value => new HouseId(value));

        builder.Property(hp => hp.PostId)
            .HasConversion(id => id.Value, value => new PostId(value));
    }
}