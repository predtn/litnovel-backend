namespace LitNovel.Application.DTOs.Novel
{
    public class UpdateNovelResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public DateTime UpdatedAt { get; set; }
    }
}
