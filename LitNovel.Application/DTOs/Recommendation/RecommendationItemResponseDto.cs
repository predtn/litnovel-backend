namespace LitNovel.Application.DTOs.Recommendation
{
    public class RecommendationItemResponseDto
    {
        public int NovelId { get; set; }
        public double Score { get; set; }
        public string Strategy { get; set; } = "personalized";
    }
}

