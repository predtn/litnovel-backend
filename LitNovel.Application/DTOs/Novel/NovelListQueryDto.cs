namespace LitNovel.Application.DTOs.Novel
{
    public class NovelListQueryDto
    {
        public int? AuthorId { get; set; }
        public string? Status { get; set; }
        public string? Sort { get; set; }
        public string? Order { get; set; }
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 20;
    }
}
