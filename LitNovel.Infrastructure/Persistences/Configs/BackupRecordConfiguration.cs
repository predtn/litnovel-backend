using LitNovel.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LitNovel.Infrastructure.Persistences.Configs
{
    public class BackupRecordConfiguration : IEntityTypeConfiguration<BackupRecord>
    {
        public void Configure(EntityTypeBuilder<BackupRecord> builder)
        {
            builder.ToTable("BackupRecords");
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Id).HasMaxLength(64);
            builder.Property(b => b.Status).HasConversion<string>().IsRequired().HasMaxLength(50);
            builder.Property(b => b.FilePath).HasMaxLength(1000);
            builder.Property(b => b.ErrorMessage).HasMaxLength(2000);

            builder.HasIndex(b => b.Status);
            builder.HasIndex(b => b.CreatedAt);
        }
    }
}
