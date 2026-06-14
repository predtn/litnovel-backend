namespace LitNovel.Application.DTOs.Admin
{
    public class AdminGeneralSettingsResponseDto
    {
        public string SiteName { get; set; } = default!;
        public string Tagline { get; set; } = default!;
        public bool MaintenanceMode { get; set; }
    }
}
