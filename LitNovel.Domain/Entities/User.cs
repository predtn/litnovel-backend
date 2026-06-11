using LitNovel.Domain.Common;
using LitNovel.Domain.Enums;

namespace LitNovel.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public string? Avatar { get; set; }
        public string? Bio { get; set; }
        public UserStatus Status { get; set; } = UserStatus.Offline;
        public UserRole Role { get; set; } = UserRole.User;

        public UserReputation? Reputation { get; set; }

        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public ICollection<UserBadge> UserBadges { get; set; } = new List<UserBadge>();
        public ICollection<Novel> Novels { get; set; } = new List<Novel>();
        public ICollection<CommentChapter> CommentChapters { get; set; } = new List<CommentChapter>();
        public ICollection<UserReport> TargetReports { get; set; } = new List<UserReport>();
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
        public ICollection<NovelLike> NovelLikes { get; set; } = new List<NovelLike>();
        public ICollection<CommentLike> CommentLikes { get; set; } = new List<CommentLike>();
        public ICollection<ReadingProgress> ReadingProgresses { get; set; } = new List<ReadingProgress>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public ICollection<NovelRating> NovelRatings { get; set; } = new List<NovelRating>();
    }
}
