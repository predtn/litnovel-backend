namespace LitNovel.Application.DTOs.Staff
{
    public class StaffDashboardActivityResponseDto
    {
        public string Action { get; set; } = default!;
        public StaffUserSummaryResponseDto Staff { get; set; } = new();
        public string Target { get; set; } = default!;
        public DateTime PerformedAt { get; set; }
    }
}
