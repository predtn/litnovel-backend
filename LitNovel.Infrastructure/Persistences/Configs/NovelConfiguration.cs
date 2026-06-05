using LitNovel.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LitNovel.Infrastructure.Persistences.Configs
{
    public class NovelConfiguration : IEntityTypeConfiguration<Novel>
    {
        public void Configure(EntityTypeBuilder<Novel> builder)
        {
            builder.ToTable("Novels");
            builder.HasKey(n => n.Id);

            builder.Property(n => n.Title).IsRequired().HasMaxLength(200);
            builder.Property(n => n.Slug).IsRequired().HasMaxLength(220);
            builder.Property(n => n.Description).HasMaxLength(5000);
            builder.Property(n => n.CoverImage).HasMaxLength(512);
            builder.Property(n => n.Status).HasConversion<string>().IsRequired();
            builder.Property(n => n.ViewCount).HasDefaultValue(0);
            builder.Property(n => n.LikeCount).HasDefaultValue(0);
            builder.Property(n => n.DislikeCount).HasDefaultValue(0);
            builder.Property(n => n.TotalChapters).HasDefaultValue(0);
            builder.Property(n => n.TotalVolumes).HasDefaultValue(0);

            builder.HasIndex(n => n.Slug).IsUnique();
            builder.HasIndex(n => n.AuthorId);
            builder.HasIndex(n => n.CategoryId);

            builder.HasOne(n => n.Author)
                .WithMany(u => u.Novels)
                .HasForeignKey(n => n.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(n => n.Category)
                .WithMany(c => c.Novels)
                .HasForeignKey(n => n.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
