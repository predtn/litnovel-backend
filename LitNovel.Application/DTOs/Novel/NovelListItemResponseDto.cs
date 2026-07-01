namespace LitNovel.Application.DTOs.Novel
{
    public class NovelListItemResponseDto
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
        public int TotalChapters { get; set; }
        public int TotalVolumes { get; set; }
        public int? LatestChapterNumber { get; set; }
        public string? LatestChapterTitle { get; set; }
        public string? LatestChapterSlug { get; set; }
        public DateTime? LatestChapterUpdatedAt { get; set; }
        public int ViewCount { get; set; }
        public int FavoritesCount { get; set; }
        public double RatingAverage { get; set; }
        public int RatingCount { get; set; }
        public DateTime? DeletionRequestedAt { get; set; }
        public DateTime? ScheduledHardDeleteAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
