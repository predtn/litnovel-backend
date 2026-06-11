namespace LitNovel.Application.DTOs.Novel
{
    public class NovelListQueryDto
    {
        public string? Keyword { get; set; }
        public int? AuthorId { get; set; }
        public int? CategoryId { get; set; }
        public List<int> TagId { get; set; } = new();
        public string? Status { get; set; }
        public string? Sort { get; set; }
        public string? Order { get; set; }
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 20;
    }
}
