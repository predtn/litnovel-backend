using LitNovel.Domain.Common;
using LitNovel.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LitNovel.Infrastructure.Persistences
{
    public class LitNovelContext : DbContext
    {
        public LitNovelContext(DbContextOptions<LitNovelContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<UserReputation> UserReputations => Set<UserReputation>();
        public DbSet<Badge> Badges => Set<Badge>();
        public DbSet<UserBadge> UserBadges => Set<UserBadge>();

        public DbSet<Novel> Novels => Set<Novel>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<NovelTag> NovelTags => Set<NovelTag>();
        public DbSet<Volume> Volumes => Set<Volume>();
        public DbSet<Chapter> Chapters => Set<Chapter>();
        public DbSet<ChapterContent> ChapterContents => Set<ChapterContent>();

        public DbSet<CommentChapter> CommentChapters => Set<CommentChapter>();
        public DbSet<Favorite> Favorites => Set<Favorite>();
        public DbSet<NovelLike> NovelLikes => Set<NovelLike>();
        public DbSet<CommentLike> CommentLikes => Set<CommentLike>();
        public DbSet<NovelRating> NovelRatings => Set<NovelRating>();
        public DbSet<ReadingProgress> ReadingProgresses => Set<ReadingProgress>();
        public DbSet<Notification> Notifications => Set<Notification>();

        public DbSet<NovelReport> NovelReports => Set<NovelReport>();
        public DbSet<UserReport> UserReports => Set<UserReport>();
        public DbSet<UserWarning> UserWarnings => Set<UserWarning>();
        public DbSet<ModerationLog> ModerationLogs => Set<ModerationLog>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LitNovelContext).Assembly);
        }

        public override int SaveChanges()
        {
            SetTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void SetTimestamps()
        {
            var entries = ChangeTracker.Entries<BaseEntity>();
            var now = DateTime.UtcNow;

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = now;
                    entry.Entity.UpdatedAt = now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = now;
                }
            }
        }
    }
}
