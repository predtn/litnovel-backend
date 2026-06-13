namespace LitNovel.Application.DTOs.Admin
{
    public class AdminStatisticsResponseDto
    {
        public AdminUserStatisticsResponseDto Users { get; set; } = new();
        public AdminNovelStatisticsResponseDto Novels { get; set; } = new();
        public AdminChapterStatisticsResponseDto Chapters { get; set; } = new();
        public AdminReportStatisticsResponseDto Reports { get; set; } = new();
        public AdminEngagementStatisticsResponseDto Engagement { get; set; } = new();
    }
}
