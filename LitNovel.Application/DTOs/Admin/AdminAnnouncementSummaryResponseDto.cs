namespace LitNovel.Application.DTOs.Admin
{
    public class AdminAnnouncementSummaryResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public bool IsActive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
