namespace LitNovel.Application.DTOs.Staff
{
    public class WarnUserRequestDto
    {
        /// <summary>Warning reason (1–1000 chars).</summary>
        public string Reason { get; set; } = default!;

        /// <summary>Minor | Major</summary>
        public string Severity { get; set; } = default!;
    }
}
