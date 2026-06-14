namespace LitNovel.Application.DTOs.Admin
{
    public class AdminSettingsResponseDto
    {
        public AdminGeneralSettingsResponseDto General { get; set; } = default!;
        public AdminContentSettingsResponseDto Content { get; set; } = default!;
        public AdminModerationSettingsResponseDto Moderation { get; set; } = default!;
    }
}
