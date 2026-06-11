using LitNovel.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LitNovel.Infrastructure.Persistences.Configs
{
    public class NovelLikeConfiguration : IEntityTypeConfiguration<NovelLike>
    {
        public void Configure(EntityTypeBuilder<NovelLike> builder)
        {
            builder.ToTable("NovelLikes");
            builder.HasKey(nl => new { nl.UserId, nl.NovelId });

            builder.Property(nl => nl.CreatedAt).IsRequired();

            builder.HasOne(nl => nl.User)
                .WithMany(u => u.NovelLikes)
                .HasForeignKey(nl => nl.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(nl => nl.Novel)
                .WithMany(n => n.NovelLikes)
                .HasForeignKey(nl => nl.NovelId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
