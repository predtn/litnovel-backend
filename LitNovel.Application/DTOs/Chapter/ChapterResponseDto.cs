namespace LitNovel.Application.DTOs.Chapter
{
    public class ChapterResponseDto
    {
        public int Id { get; set; }
        public int ChapterNumber { get; set; }
        public string Title { get; set; } = default!;
        public string Status { get; set; } = default!;
        public int VolumeId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
