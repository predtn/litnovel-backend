namespace LitNovel.Application.DTOs.Admin
{
    public class UpdateAdminGeneralSettingsRequestDto
    {
        public string? SiteName { get; set; }
        public string? Tagline { get; set; }
        public bool? MaintenanceMode { get; set; }
    }
}
