using LitNovel.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LitNovel.Infrastructure.Persistences.Configs
{
    public class ChapterReadConfiguration : IEntityTypeConfiguration<ChapterRead>
    {
        public void Configure(EntityTypeBuilder<ChapterRead> builder)
        {
            builder.ToTable("ChapterReads");
            builder.HasKey(cr => new { cr.UserId, cr.ChapterId });

            builder.Property(cr => cr.ReadAt).IsRequired();

            builder.HasIndex(cr => new { cr.UserId, cr.NovelId });
            builder.HasIndex(cr => cr.ChapterId);

            builder.HasOne(cr => cr.User)
                .WithMany(u => u.ChapterReads)
                .HasForeignKey(cr => cr.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(cr => cr.Novel)
                .WithMany(n => n.ChapterReads)
                .HasForeignKey(cr => cr.NovelId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(cr => cr.Chapter)
                .WithMany(c => c.ChapterReads)
                .HasForeignKey(cr => cr.ChapterId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
