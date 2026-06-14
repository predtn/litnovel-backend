namespace LitNovel.Application.DTOs.Admin
{
    public class AdminModerationSettingsResponseDto
    {
        public int ReviewSLAHours { get; set; }
        public IReadOnlyList<string> AutoFlagKeywords { get; set; } = Array.Empty<string>();
    }
}
