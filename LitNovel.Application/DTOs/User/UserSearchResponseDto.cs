namespace LitNovel.Application.DTOs.User
{
    public class UserSearchResponseDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = default!;
        public string? Avatar { get; set; }
        public int NovelsPublished { get; set; }
    }
}
