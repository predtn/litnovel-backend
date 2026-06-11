namespace LitNovel.Application.DTOs.Novel
{
    public class NovelTopChapterResponseDto
    {
        public int ChapterId { get; set; }
        public string Title { get; set; } = default!;
        public int CommentCount { get; set; }
    }
}
