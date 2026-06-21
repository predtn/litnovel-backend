namespace LitNovel.Application.DTOs.Admin
{
    public class UpdateAdminNovelAuthorRequestDto
    {
        public int AuthorId { get; set; }
        public string? Reason { get; set; }
    }
}
