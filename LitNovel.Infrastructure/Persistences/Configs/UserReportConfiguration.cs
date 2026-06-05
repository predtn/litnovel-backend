using LitNovel.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LitNovel.Infrastructure.Persistences.Configs
{
    public class UserReportConfiguration : IEntityTypeConfiguration<UserReport>
    {
        public void Configure(EntityTypeBuilder<UserReport> builder)
        {
            builder.ToTable("UserReports");
            builder.HasKey(r => r.Id);

            builder.Property(r => r.ReportType).HasConversion<string>().IsRequired();
            builder.Property(r => r.Status).HasConversion<string>().IsRequired();
            builder.Property(r => r.Description).HasMaxLength(2000);
            builder.Property(r => r.ActionTaken).HasMaxLength(1000);
            builder.Property(r => r.ResolutionNotes).HasMaxLength(1000);

            builder.HasIndex(r => new { r.ReporterId, r.TargetUserId });

            builder.HasOne(r => r.Reporter)
                .WithMany()
                .HasForeignKey(r => r.ReporterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.ProcessedBy)
                .WithMany()
                .HasForeignKey(r => r.ProcessedById)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);

            builder.HasOne(r => r.TargetUser)
                .WithMany(u => u.TargetReports)
                .HasForeignKey(r => r.TargetUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.TargetComment)
                .WithMany(c => c.TargetReports)
                .HasForeignKey(r => r.TargetCommentId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);
        }
    }
}
