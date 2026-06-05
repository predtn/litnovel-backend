using LitNovel.Domain.Common;
using LitNovel.Domain.Enums;

namespace LitNovel.Domain.Entities
{
    public class Chapter : BaseEntity
    {
        public int VolumeId { get; set; }
        public int ChapterNumber { get; set; }
        public string Title { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public DateTime? ReleaseDate { get; set; }
        public ChapterStatus Status { get; set; } = ChapterStatus.Draft;

        public Volume Volume { get; set; } = default!;
        public ChapterContent? Content { get; set; }

        public ICollection<CommentChapter> CommentChapters { get; set; } = new List<CommentChapter>();
        public ICollection<ReadingProgress> ChapterProgresses { get; set; } = new List<ReadingProgress>();
    }
}
