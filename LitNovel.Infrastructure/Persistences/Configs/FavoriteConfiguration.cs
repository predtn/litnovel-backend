using LitNovel.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LitNovel.Infrastructure.Persistences.Configs
{
    public class FavoriteConfiguration : IEntityTypeConfiguration<Favorite>
    {
        public void Configure(EntityTypeBuilder<Favorite> builder)
        {
            builder.ToTable("Favorites");
            builder.HasKey(f => new { f.UserId, f.NovelId });

            builder.Property(f => f.CreatedAt).IsRequired();

            builder.HasOne(f => f.User)
                .WithMany(u => u.Favorites)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(f => f.Novel)
                .WithMany(n => n.Favorites)
                .HasForeignKey(f => f.NovelId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
