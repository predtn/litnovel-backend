using LitNovel.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LitNovel.Infrastructure.Persistences.Configs
{
    public class ChapterContentConfiguration : IEntityTypeConfiguration<ChapterContent>
    {
        public void Configure(EntityTypeBuilder<ChapterContent> builder)
        {
            builder.ToTable("ChapterContents");
            builder.HasKey(cc => cc.Id);

            builder.Property(cc => cc.Content).IsRequired().HasColumnType("nvarchar(max)");
            builder.Property(cc => cc.Version).IsRequired().HasDefaultValue(1);

            builder.HasIndex(cc => cc.ChapterId).IsUnique();

            builder.HasOne(cc => cc.Chapter)
                .WithOne(c => c.Content)
                .HasForeignKey<ChapterContent>(cc => cc.ChapterId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
