namespace LitNovel.Application.DTOs.Novel
{
    public class NovelAnalyticsResponseDto
    {
        public int NovelId { get; set; }
        public int ViewCount { get; set; }
        public int LikeCount { get; set; }
        public int FavoritesCount { get; set; }
        public double RatingAverage { get; set; }
        public int RatingCount { get; set; }
        public int CommentCount { get; set; }
        public Dictionary<int, int> RatingDistribution { get; set; } = new();
        public List<NovelTopChapterResponseDto> TopChapters { get; set; } = new();
    }
}
