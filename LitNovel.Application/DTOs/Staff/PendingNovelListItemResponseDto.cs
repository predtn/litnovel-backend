namespace LitNovel.Application.DTOs.Staff
{
    public class PendingNovelListItemResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public string? CoverImage { get; set; }
        public string? Description { get; set; }
        public int AuthorId { get; set; }
        public string AuthorUsername { get; set; } = default!;
        public string? CategoryName { get; set; }
        public List<string> Tags { get; set; } = [];
        public int TotalChapters { get; set; }
        public DateTime SubmittedAt { get; set; }
    }
}
