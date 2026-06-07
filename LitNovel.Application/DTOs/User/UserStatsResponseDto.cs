namespace LitNovel.Application.DTOs.User
{
    public class UserStatsResponseDto
    {
        public int NovelsCreated { get; set; }
        public int NovelsPublished { get; set; }
        public int ChaptersPublished { get; set; }
        public int FavoritesCount { get; set; }
        public int CommentsCount { get; set; }
    }
}
