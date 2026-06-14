using LitNovel.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LitNovel.Infrastructure.Persistences.Configs
{
    public class AnnouncementConfiguration : IEntityTypeConfiguration<Announcement>
    {
        public void Configure(EntityTypeBuilder<Announcement> builder)
        {
            builder.ToTable("Announcements");
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Title).IsRequired().HasMaxLength(200);
            builder.Property(a => a.Content).IsRequired().HasMaxLength(2000);
            builder.Property(a => a.IsActive).HasDefaultValue(true);

            builder.HasIndex(a => a.IsActive);
            builder.HasIndex(a => a.StartDate);
            builder.HasIndex(a => a.EndDate);
        }
    }
}
