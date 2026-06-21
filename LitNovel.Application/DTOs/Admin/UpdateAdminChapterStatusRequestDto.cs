namespace LitNovel.Application.DTOs.Admin
{
    public class UpdateAdminChapterStatusRequestDto
    {
        public string Status { get; set; } = default!;
        public string? Reason { get; set; }
    }
}
