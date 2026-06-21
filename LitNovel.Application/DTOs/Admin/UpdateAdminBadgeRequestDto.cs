namespace LitNovel.Application.DTOs.Admin
{
    public class UpdateAdminBadgeRequestDto
    {
        public string? Key { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public string? Color { get; set; }
    }
}
