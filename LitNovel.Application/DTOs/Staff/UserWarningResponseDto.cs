namespace LitNovel.Application.DTOs.Staff
{
    public class UserWarningResponseDto
    {
        public int Id { get; set; }
        public string Reason { get; set; } = default!;
        public string Severity { get; set; } = default!;
        public int IssuedById { get; set; }
        public string IssuedByUsername { get; set; } = default!;
        public DateTime IssuedAt { get; set; }
    }
}
