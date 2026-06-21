namespace LitNovel.Application.DTOs.Admin
{
    public class UpdateAdminSettingsRequestDto
    {
        public UpdateAdminGeneralSettingsRequestDto? General { get; set; }
        public UpdateAdminContentSettingsRequestDto? Content { get; set; }
        public UpdateAdminModerationSettingsRequestDto? Moderation { get; set; }
    }
}
