using LitNovel.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LitNovel.Infrastructure.Persistences.Configs
{
    public class UserWarningConfiguration : IEntityTypeConfiguration<UserWarning>
    {
        public void Configure(EntityTypeBuilder<UserWarning> builder)
        {
            builder.ToTable("UserWarnings");
            builder.HasKey(w => w.Id);

            builder.Property(w => w.Reason).HasMaxLength(1000).IsRequired();
            builder.Property(w => w.Severity).HasConversion<string>().IsRequired();

            builder.HasIndex(w => w.UserId);
            builder.HasIndex(w => w.IssuedById);

            builder.HasOne(w => w.User)
                .WithMany()
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(w => w.IssuedBy)
                .WithMany()
                .HasForeignKey(w => w.IssuedById)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
