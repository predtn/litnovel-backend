namespace LitNovel.Application.DTOs.Novel
{
    public class CreateNovelRequestDto
    {
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public string? CoverImage { get; set; }
        public int? CategoryId { get; set; }
        public List<int>? TagIds { get; set; }
    }
}
