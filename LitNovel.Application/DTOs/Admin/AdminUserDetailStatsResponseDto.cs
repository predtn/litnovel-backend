namespace LitNovel.Application.DTOs.Admin
{
    public class AdminUserDetailStatsResponseDto
    {
        public int NovelsCreated { get; set; }
        public int ChaptersPublished { get; set; }
        public int CommentsCount { get; set; }
        public int ReportsReceived { get; set; }
        public int WarningsCount { get; set; }
    }
}
