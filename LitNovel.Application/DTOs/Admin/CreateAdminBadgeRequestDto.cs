namespace LitNovel.Application.DTOs.Admin
{
    public class CreateAdminBadgeRequestDto
    {
        public string Key { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string? Icon { get; set; }
        public string? Color { get; set; }
    }
}
