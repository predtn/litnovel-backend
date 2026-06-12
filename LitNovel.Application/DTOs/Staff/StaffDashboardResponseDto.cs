namespace LitNovel.Application.DTOs.Staff
{
    public class StaffDashboardResponseDto
    {
        public int PendingNovels { get; set; }
        public int PendingChapters { get; set; }
        public int OpenReports { get; set; }
        public int ActiveWarnings { get; set; }
        public List<ModerationActivityDto> RecentActivity { get; set; } = [];
    }

    public class ModerationActivityDto
    {
        public string Action { get; set; } = default!;
        public string StaffUsername { get; set; } = default!;
        public string Target { get; set; } = default!;
        public DateTime PerformedAt { get; set; }
    }
}
