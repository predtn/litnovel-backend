using LitNovel.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LitNovel.Infrastructure.Persistences.Configs
{
    public class CommentLikeConfiguration : IEntityTypeConfiguration<CommentLike>
    {
        public void Configure(EntityTypeBuilder<CommentLike> builder)
        {
            builder.ToTable("CommentLikes");
            builder.HasKey(cl => new { cl.UserId, cl.CommentChapterId });

            builder.Property(cl => cl.CreatedAt).IsRequired();

            builder.HasOne(cl => cl.User)
                .WithMany(u => u.CommentLikes)
                .HasForeignKey(cl => cl.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(cl => cl.CommentChapter)
                .WithMany(c => c.CommentLikes)
                .HasForeignKey(cl => cl.CommentChapterId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
