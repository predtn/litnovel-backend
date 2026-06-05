using LitNovel.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LitNovel.Infrastructure.Persistences.Configs
{
    public class UserReputationConfiguration : IEntityTypeConfiguration<UserReputation>
    {
        public void Configure(EntityTypeBuilder<UserReputation> builder)
        {
            builder.ToTable("UserReputations");
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Score).IsRequired().HasDefaultValue(0);
            builder.Property(r => r.UpdatedAt).IsRequired();

            builder.HasIndex(r => r.UserId).IsUnique();
        }
    }
}
