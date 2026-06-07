namespace LitNovel.Application.DTOs.Tag
{
    public class TagResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Slug { get; set; } = default!;
    }
}
