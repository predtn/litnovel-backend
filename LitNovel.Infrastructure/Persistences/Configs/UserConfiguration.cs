using LitNovel.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LitNovel.Infrastructure.Persistences.Configs
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Username).IsRequired().HasMaxLength(50);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(256);
            builder.Property(u => u.PasswordHash).IsRequired();
            builder.Property(u => u.Avatar).HasMaxLength(512);
            builder.Property(u => u.Bio).HasMaxLength(1000);
            builder.Property(u => u.Status).HasConversion<string>().IsRequired();
            builder.Property(u => u.Role).HasConversion<string>().IsRequired();

            builder.HasIndex(u => u.Username).IsUnique();
            builder.HasIndex(u => u.Email).IsUnique();

            builder.HasOne(u => u.Reputation)
                .WithOne(r => r.User)
                .HasForeignKey<UserReputation>(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
