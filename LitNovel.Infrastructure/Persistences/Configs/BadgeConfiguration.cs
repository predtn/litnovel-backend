using LitNovel.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LitNovel.Infrastructure.Persistences.Configs
{
    public class BadgeConfiguration : IEntityTypeConfiguration<Badge>
    {
        public void Configure(EntityTypeBuilder<Badge> builder)
        {
            builder.ToTable("Badges");
            builder.HasKey(bg => bg.Id);

            builder.Property(bg => bg.Key).IsRequired().HasMaxLength(100);
            builder.Property(bg => bg.Name).IsRequired().HasMaxLength(100);
            builder.Property(bg => bg.Description).IsRequired().HasMaxLength(500);
            builder.Property(bg => bg.Icon).HasMaxLength(512);
            builder.Property(bg => bg.Color).HasMaxLength(20);

            builder.HasIndex(bg => bg.Key).IsUnique();
        }
    }
}
