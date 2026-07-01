namespace LitNovel.Application.DTOs.Novel
{
    public class NovelDetailResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public string? CoverImage { get; set; }
        public string? Description { get; set; }
        public NovelAuthorResponseDto Author { get; set; } = default!;
        public NovelCategoryResponseDto? Category { get; set; }
        public List<NovelTagResponseDto> Tags { get; set; } = new();
        public string Status { get; set; } = default!;
        public int ViewCount { get; set; }
        public int LikeCount { get; set; }
        public bool? IsFavorited { get; set; }
        public bool? IsLiked { get; set; }
        public int? UserReviewId { get; set; }
        public int? UserRating { get; set; }
        public string? UserReview { get; set; }
        public int TotalChapters { get; set; }
        public int TotalVolumes { get; set; }
        public int ReadChapterCount { get; set; }
        public int ReadingProgressPercentage { get; set; }
        public double RatingAverage { get; set; }
        public int RatingCount { get; set; }
        public List<NovelDetailVolumeResponseDto> Volumes { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class NovelDetailVolumeResponseDto
    {
        public int Id { get; set; }
        public int VolumeNumber { get; set; }
        public string Title { get; set; } = default!;
        public List<NovelDetailChapterResponseDto> Chapters { get; set; } = new();
    }

    public class NovelDetailChapterResponseDto
    {
        public int Id { get; set; }
        public string Slug { get; set; } = default!;
        public int ChapterNumber { get; set; }
        public string Title { get; set; } = default!;
        public string Status { get; set; } = default!;
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
