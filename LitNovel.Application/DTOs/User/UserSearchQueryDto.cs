namespace LitNovel.Application.DTOs.User
{
    public class UserSearchQueryDto
    {
        public string? Keyword { get; set; }
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
    }
}
