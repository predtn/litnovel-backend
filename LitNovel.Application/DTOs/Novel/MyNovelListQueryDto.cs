namespace LitNovel.Application.DTOs.Novel
{
    public class MyNovelListQueryDto
    {
        public string? Status { get; set; }
        public string? Sort { get; set; }
        public string? Order { get; set; }
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 20;
    }
}
