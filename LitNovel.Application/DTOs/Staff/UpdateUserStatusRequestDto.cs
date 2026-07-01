using System.ComponentModel.DataAnnotations;

namespace LitNovel.Application.DTOs.Staff
{
    public class UpdateUserStatusRequestDto
    {
        [Required]
        public string Status { get; set; } = string.Empty;

        [Required(ErrorMessage = "Lý do khóa/mở khóa là bắt buộc.")]
        [MinLength(5, ErrorMessage = "Lý do phải có ít nhất 5 ký tự.")]
        public string Reason { get; set; } = string.Empty;
    }
}
