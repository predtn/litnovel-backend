namespace LitNovel.Application.DTOs.Reading
{
    public class SaveReadingProgressResponseDto
    {
        public int NovelId { get; set; }
        public int ChapterId { get; set; }
        public int ProgressPercentage { get; set; }
        public DateTime LastReadAt { get; set; }
    }
}
