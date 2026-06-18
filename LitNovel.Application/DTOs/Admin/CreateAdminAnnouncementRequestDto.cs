namespace LitNovel.Application.DTOs.Admin
{
    public class CreateAdminAnnouncementRequestDto
    {
        public string Title { get; set; } = default!;
        public string Content { get; set; } = default!;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsActive { get; set; }
    }
}
