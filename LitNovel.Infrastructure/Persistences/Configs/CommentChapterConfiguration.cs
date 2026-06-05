using LitNovel.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LitNovel.Infrastructure.Persistences.Configs
{
    public class CommentChapterConfiguration : IEntityTypeConfiguration<CommentChapter>
    {
        public void Configure(EntityTypeBuilder<CommentChapter> builder)
        {
            builder.ToTable("CommentChapters");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Content).IsRequired().HasMaxLength(2000);
            builder.Property(c => c.LikeCount).HasDefaultValue(0);
            builder.Property(c => c.DislikeCount).HasDefaultValue(0);

            builder.HasIndex(c => c.ChapterId);
            builder.HasIndex(c => c.UserId);

            builder.HasOne(c => c.User)
                .WithMany(u => u.CommentChapters)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Chapter)
                .WithMany(ch => ch.CommentChapters)
                .HasForeignKey(c => c.ChapterId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.ParentComment)
                .WithMany(c => c.Replies)
                .HasForeignKey(c => c.ParentCommentId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
        }
    }
}
