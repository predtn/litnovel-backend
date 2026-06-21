using LitNovel.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LitNovel.Infrastructure.Persistences.Configs
{
    public class ModerationLogConfiguration : IEntityTypeConfiguration<ModerationLog>
    {
        public void Configure(EntityTypeBuilder<ModerationLog> builder)
        {
            builder.ToTable("ModerationLogs");
            builder.HasKey(l => l.Id);

            builder.Property(l => l.Action).HasMaxLength(100).IsRequired();
            builder.Property(l => l.TargetType).HasMaxLength(50).IsRequired();
            builder.Property(l => l.TargetTitle).HasMaxLength(300);
            builder.Property(l => l.Notes).HasMaxLength(1000);

            builder.HasIndex(l => l.StaffId);
            builder.HasIndex(l => l.PerformedAt);
            builder.HasIndex(l => l.Action);

            builder.HasOne(l => l.Staff)
                .WithMany()
                .HasForeignKey(l => l.StaffId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
