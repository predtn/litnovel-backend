namespace LitNovel.Application.DTOs.Staff
{
    public class ChapterReviewDetailResponseDto
    {
        public int Id { get; set; }
        public int ChapterNumber { get; set; }
        public string Title { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public string Status { get; set; } = default!;
        public string Content { get; set; } = string.Empty;
        public int NovelId { get; set; }
        public string NovelTitle { get; set; } = default!;
        public string NovelSlug { get; set; } = default!;
        public int AuthorId { get; set; }
        public string AuthorUsername { get; set; } = default!;
        public int VolumeId { get; set; }
        public string VolumeTitle { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
