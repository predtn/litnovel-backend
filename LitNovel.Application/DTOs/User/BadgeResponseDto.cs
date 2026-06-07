namespace LitNovel.Application.DTOs.User
{
    public class BadgeResponseDto
    {
        public string Key { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Icon { get; set; }
        public string? Color { get; set; }
        public DateTime EarnedAt { get; set; }
    }
}
