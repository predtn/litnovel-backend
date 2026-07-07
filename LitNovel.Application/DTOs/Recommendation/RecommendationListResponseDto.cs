using LitNovel.Application.DTOs.Novel;

namespace LitNovel.Application.DTOs.Recommendation
{
    public class RecommendationListResponseDto
    {
        public string Strategy { get; set; } = "personalized";
        public List<NovelListItemResponseDto> Items { get; set; } = new();
    }
}

