using LitNovel.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LitNovel.Infrastructure.Persistences.Configs
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notifications");
            builder.HasKey(n => n.Id);

            builder.Property(n => n.NotificationType).HasConversion<string>().IsRequired();
            builder.Property(n => n.Message).IsRequired().HasMaxLength(1000);
            builder.Property(n => n.EntityType).HasMaxLength(100);
            builder.Property(n => n.IsRead).HasDefaultValue(false);

            builder.HasIndex(n => new { n.UserId, n.IsRead });

            builder.HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
