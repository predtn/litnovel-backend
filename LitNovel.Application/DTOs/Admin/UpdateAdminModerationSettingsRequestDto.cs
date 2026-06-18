namespace LitNovel.Application.DTOs.Admin
{
    public class UpdateAdminModerationSettingsRequestDto
    {
        public int? ReviewSLAHours { get; set; }
        public IReadOnlyList<string>? AutoFlagKeywords { get; set; }
    }
}
