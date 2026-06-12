namespace LitNovel.Application.DTOs.Staff
{
    public class NovelReviewDetailResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public string? Description { get; set; }
        public string? CoverImage { get; set; }
        public string Status { get; set; } = default!;
        public int AuthorId { get; set; }
        public string AuthorUsername { get; set; } = default!;
        public string? AuthorAvatar { get; set; }
        public string? CategoryName { get; set; }
        public List<string> Tags { get; set; } = [];
        public int TotalChapters { get; set; }
        public int TotalVolumes { get; set; }
        public int ViewCount { get; set; }
        public List<NovelReviewVolumeDto> Volumes { get; set; } = [];
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class NovelReviewVolumeDto
    {
        public int Id { get; set; }
        public int VolumeNumber { get; set; }
        public string Title { get; set; } = default!;
        public List<NovelReviewChapterDto> Chapters { get; set; } = [];
    }

    public class NovelReviewChapterDto
    {
        public int Id { get; set; }
        public int ChapterNumber { get; set; }
        public string Title { get; set; } = default!;
        public string Status { get; set; } = default!;
    }
}
