namespace LitNovel.Application.DTOs.Novel
{
    public class MyNovelListItemResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public string? CoverImage { get; set; }
        public string Status { get; set; } = default!;
        public int TotalChapters { get; set; }
        public int TotalVolumes { get; set; }
        public int ViewCount { get; set; }
        public double RatingAverage { get; set; }
        public int RatingCount { get; set; }
        public DateTime? DeletionRequestedAt { get; set; }
        public DateTime? ScheduledHardDeleteAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
