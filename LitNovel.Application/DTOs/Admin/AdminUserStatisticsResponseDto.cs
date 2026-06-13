namespace LitNovel.Application.DTOs.Admin
{
    public class AdminUserStatisticsResponseDto
    {
        public int Total { get; set; }
        public int NewThisWeek { get; set; }
        public int Banned { get; set; }
    }
}
