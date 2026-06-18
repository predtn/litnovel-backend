namespace LitNovel.Application.DTOs.Admin
{
    public class AdminStatisticsChartResponseDto
    {
        public string Metric { get; set; } = string.Empty;
        public IReadOnlyList<AdminStatisticsChartPointResponseDto> Points { get; set; } =
            Array.Empty<AdminStatisticsChartPointResponseDto>();
    }
}
