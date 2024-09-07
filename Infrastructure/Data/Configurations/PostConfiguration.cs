using Domain;
using Domain.Entities;
using Domain.StronglyTypedIds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public sealed class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.Property(h => h.Id)
            .HasConversion(id => id.Value, value => new PostId(value))
            .ValueGeneratedOnAdd();
        
        builder.Property(post => post.Name)
            .HasMaxLength(DomainConstants.MaxPostNameLength)
            .IsRequired();

        builder.HasIndex(post => post.Name)
            .IsUnique();
    }
}