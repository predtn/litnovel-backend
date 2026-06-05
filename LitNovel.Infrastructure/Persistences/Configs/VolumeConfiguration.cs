using LitNovel.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LitNovel.Infrastructure.Persistences.Configs
{
    public class VolumeConfiguration : IEntityTypeConfiguration<Volume>
    {
        public void Configure(EntityTypeBuilder<Volume> builder)
        {
            builder.ToTable("Volumes");
            builder.HasKey(v => v.Id);

            builder.Property(v => v.Title).IsRequired().HasMaxLength(200);
            builder.Property(v => v.VolumeNumber).IsRequired();

            builder.HasIndex(v => new { v.NovelId, v.VolumeNumber }).IsUnique();

            builder.HasOne(v => v.Novel)
                .WithMany(n => n.Volumes)
                .HasForeignKey(v => v.NovelId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
