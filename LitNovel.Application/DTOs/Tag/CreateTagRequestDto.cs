namespace LitNovel.Application.DTOs.Tag
{
    public class CreateTagRequestDto
    {
        public string Name { get; set; } = default!;
        public string Slug { get; set; } = default!;
    }
}
