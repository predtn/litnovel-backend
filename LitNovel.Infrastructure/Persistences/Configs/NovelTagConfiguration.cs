using LitNovel.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LitNovel.Infrastructure.Persistences.Configs
{
    public class NovelTagConfiguration : IEntityTypeConfiguration<NovelTag>
    {
        public void Configure(EntityTypeBuilder<NovelTag> builder)
        {
            builder.ToTable("NovelTags");
            builder.HasKey(nt => new { nt.NovelId, nt.TagId });

            builder.HasOne(nt => nt.Novel)
                .WithMany(n => n.NovelTags)
                .HasForeignKey(nt => nt.NovelId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(nt => nt.Tag)
                .WithMany(t => t.NovelTags)
                .HasForeignKey(nt => nt.TagId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
