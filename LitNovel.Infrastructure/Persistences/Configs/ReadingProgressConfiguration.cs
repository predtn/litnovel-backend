using LitNovel.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LitNovel.Infrastructure.Persistences.Configs
{
    public class ReadingProgressConfiguration : IEntityTypeConfiguration<ReadingProgress>
    {
        public void Configure(EntityTypeBuilder<ReadingProgress> builder)
        {
            builder.ToTable("ReadingProgresses");
            builder.HasKey(rp => new { rp.UserId, rp.NovelId });

            builder.Property(rp => rp.ProgressPercentage).IsRequired();
            builder.Property(rp => rp.LastReadAt).IsRequired();

            builder.HasIndex(rp => rp.UserId);

            builder.HasOne(rp => rp.User)
                .WithMany(u => u.ReadingProgresses)
                .HasForeignKey(rp => rp.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(rp => rp.Novel)
                .WithMany(n => n.NovelProgresses)
                .HasForeignKey(rp => rp.NovelId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(rp => rp.Chapter)
                .WithMany(c => c.ChapterProgresses)
                .HasForeignKey(rp => rp.ChapterId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
