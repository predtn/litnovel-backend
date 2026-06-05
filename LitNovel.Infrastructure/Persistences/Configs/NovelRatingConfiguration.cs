using LitNovel.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LitNovel.Infrastructure.Persistences.Configs
{
    public class NovelRatingConfiguration : IEntityTypeConfiguration<NovelRating>
    {
        public void Configure(EntityTypeBuilder<NovelRating> builder)
        {
            builder.ToTable("NovelRatings");
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Rating).IsRequired();
            builder.Property(r => r.Review).HasMaxLength(3000);
            builder.Property(r => r.UpdatedAt).IsRequired(false);

            builder.HasIndex(r => new { r.UserId, r.NovelId }).IsUnique();

            builder.HasOne(r => r.Novel)
                .WithMany(n => n.NovelRatings)
                .HasForeignKey(r => r.NovelId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.User)
                .WithMany(u => u.NovelRatings)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
