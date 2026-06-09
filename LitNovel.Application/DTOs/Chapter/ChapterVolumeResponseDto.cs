namespace LitNovel.Application.DTOs.Chapter
{
    public class ChapterVolumeResponseDto
    {
        public int Id { get; set; }
        public int VolumeNumber { get; set; }
        public string Title { get; set; } = default!;
    }
}
