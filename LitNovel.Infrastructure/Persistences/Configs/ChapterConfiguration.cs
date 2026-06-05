using LitNovel.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LitNovel.Infrastructure.Persistences.Configs
{
    public class ChapterConfiguration : IEntityTypeConfiguration<Chapter>
    {
        public void Configure(EntityTypeBuilder<Chapter> builder)
        {
            builder.ToTable("Chapters");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Title).IsRequired().HasMaxLength(200);
            builder.Property(c => c.Slug).IsRequired().HasMaxLength(220);
            builder.Property(c => c.ChapterNumber).IsRequired();
            builder.Property(c => c.Status).HasConversion<string>().IsRequired();
            builder.Property(c => c.ReleaseDate).IsRequired(false);

            builder.HasIndex(c => new { c.VolumeId, c.ChapterNumber }).IsUnique();
            builder.HasIndex(c => c.Slug).IsUnique();

            builder.HasOne(c => c.Volume)
                .WithMany(v => v.Chapters)
                .HasForeignKey(c => c.VolumeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
