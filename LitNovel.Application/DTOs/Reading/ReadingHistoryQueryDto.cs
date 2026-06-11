namespace LitNovel.Application.DTOs.Reading
{
    public class ReadingHistoryQueryDto
    {
        public string? Filter { get; set; } = "all";
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 20;
    }
}
