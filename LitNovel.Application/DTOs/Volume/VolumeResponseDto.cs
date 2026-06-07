namespace LitNovel.Application.DTOs.Volume
{
    public class VolumeResponseDto
    {
        public int Id { get; set; }
        public int VolumeNumber { get; set; }
        public string Title { get; set; } = default!;
        public int ChapterCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
