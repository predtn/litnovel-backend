namespace LitNovel.Application.DTOs.Chapter
{
    public class ChapterListQueryDto
    {
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 50;
        public string? Status { get; set; }
        public string? Sort { get; set; } = "chapterNumber";
        public string? Order { get; set; } = "asc";
    }
}
