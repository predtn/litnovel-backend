namespace LitNovel.Application.DTOs.Admin
{
    public class AdminStatisticsChartQueryDto
    {
        public string Metric { get; set; } = string.Empty;
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string Granularity { get; set; } = "day";
    }
}
