namespace LitNovel.Application.DTOs.Admin
{
    public class UpdateAdminNovelStatusRequestDto
    {
        public string Status { get; set; } = default!;
        public string? Reason { get; set; }
    }
}
