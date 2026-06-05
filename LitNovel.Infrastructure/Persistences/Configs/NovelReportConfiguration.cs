using LitNovel.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LitNovel.Infrastructure.Persistences.Configs
{
    public class NovelReportConfiguration : IEntityTypeConfiguration<NovelReport>
    {
        public void Configure(EntityTypeBuilder<NovelReport> builder)
        {
            builder.ToTable("NovelReports");
            builder.HasKey(r => r.Id);

            builder.Property(r => r.ReportType).HasConversion<string>().IsRequired();
            builder.Property(r => r.Status).HasConversion<string>().IsRequired();
            builder.Property(r => r.Description).HasMaxLength(2000);
            builder.Property(r => r.ActionTaken).HasMaxLength(1000);
            builder.Property(r => r.ResolutionNotes).HasMaxLength(1000);

            builder.HasIndex(r => new { r.ReporterId, r.TargetNovelId });

            builder.HasOne(r => r.Reporter)
                .WithMany()
                .HasForeignKey(r => r.ReporterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.ProcessedBy)
                .WithMany()
                .HasForeignKey(r => r.ProcessedById)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);

            builder.HasOne(r => r.TargetNovel)
                .WithMany(n => n.TargetReports)
                .HasForeignKey(r => r.TargetNovelId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.TargetChapter)
                .WithMany()
                .HasForeignKey(r => r.TargetChapterId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);
        }
    }
}
