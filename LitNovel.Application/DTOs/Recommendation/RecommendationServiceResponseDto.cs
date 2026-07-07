namespace LitNovel.Application.DTOs.Recommendation
{
    public class RecommendationServiceResponseDto
    {
        public int UserId { get; set; }
        public string Strategy { get; set; } = "personalized";
        public List<RecommendationItemResponseDto> Items { get; set; } = new();
    }
}

