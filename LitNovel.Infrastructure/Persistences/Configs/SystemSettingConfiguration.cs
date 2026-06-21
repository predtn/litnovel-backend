using LitNovel.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LitNovel.Infrastructure.Persistences.Configs
{
    public class SystemSettingConfiguration : IEntityTypeConfiguration<SystemSetting>
    {
        public void Configure(EntityTypeBuilder<SystemSetting> builder)
        {
            builder.ToTable("SystemSettings");
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Key).IsRequired().HasMaxLength(100);
            builder.Property(s => s.Value).IsRequired().HasMaxLength(4000);

            builder.HasIndex(s => s.Key).IsUnique();
        }
    }
}
