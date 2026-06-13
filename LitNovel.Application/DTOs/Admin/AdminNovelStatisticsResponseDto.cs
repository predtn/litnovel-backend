namespace LitNovel.Application.DTOs.Admin
{
    public class AdminNovelStatisticsResponseDto
    {
        public int Total { get; set; }
        public int Ongoing { get; set; }
        public int Pending { get; set; }
        public int NewThisMonth { get; set; }
    }
}
