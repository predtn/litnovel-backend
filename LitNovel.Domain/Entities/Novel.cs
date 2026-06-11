using LitNovel.Domain.Common;
using LitNovel.Domain.Enums;

namespace LitNovel.Domain.Entities
{
    public class Novel : BaseEntity
    {
        public string Title { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public string? Description { get; set; }
        public string? CoverImage { get; set; }
        public int AuthorId { get; set; }
        public NovelStatus Status { get; set; } = NovelStatus.Pending;
        public int ViewCount { get; set; } = 0;
        public int LikeCount { get; set; } = 0;
        public int DislikeCount { get; set; } = 0;
        public int? CategoryId { get; set; }
        public int TotalChapters { get; set; } = 0;
        public int TotalVolumes { get; set; } = 0;

        public User Author { get; set; } = default!;
        public Category? Category { get; set; }

        public ICollection<Volume> Volumes { get; set; } = new List<Volume>();
        public ICollection<NovelTag> NovelTags { get; set; } = new List<NovelTag>();
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
        public ICollection<NovelLike> NovelLikes { get; set; } = new List<NovelLike>();
        public ICollection<NovelRating> NovelRatings { get; set; } = new List<NovelRating>();
        public ICollection<NovelReport> TargetReports { get; set; } = new List<NovelReport>();
        public ICollection<ReadingProgress> NovelProgresses { get; set; } = new List<ReadingProgress>();
    }
}
