namespace LitNovel.Application.DTOs.Category
{
    public class CreateCategoryRequestDto
    {
        public string Name { get; set; } = default!;
        public string Slug { get; set; } = default!;
    }
}
