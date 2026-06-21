namespace LitNovel.Application.DTOs.Staff
{
    public class ResolveReportRequestDto
    {
        /// <summary>Resolve | Reject</summary>
        public string Action { get; set; } = default!;
        public string? ActionTaken { get; set; }
        public string? ResolutionNotes { get; set; }
    }
}
