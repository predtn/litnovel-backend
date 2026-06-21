using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Admin;
using LitNovel.WebAPI.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LitNovel.WebAPI.Controllers
{
    [ApiController]
    [Route("api/admin/backups")]
    [Authorize(Roles = "Admin")]
    public class AdminBackupsController : ControllerBase
    {
        private readonly IGetAdminBackupsUseCase _getBackupsUseCase;
        private readonly ICreateAdminBackupUseCase _createBackupUseCase;
        private readonly IDownloadAdminBackupUseCase _downloadBackupUseCase;
        private readonly IRestoreAdminBackupUseCase _restoreBackupUseCase;
        private readonly IDeleteAdminBackupUseCase _deleteBackupUseCase;

        public AdminBackupsController(
            IGetAdminBackupsUseCase getBackupsUseCase,
            ICreateAdminBackupUseCase createBackupUseCase,
            IDownloadAdminBackupUseCase downloadBackupUseCase,
            IRestoreAdminBackupUseCase restoreBackupUseCase,
            IDeleteAdminBackupUseCase deleteBackupUseCase)
        {
            _getBackupsUseCase = getBackupsUseCase;
            _createBackupUseCase = createBackupUseCase;
            _downloadBackupUseCase = downloadBackupUseCase;
            _restoreBackupUseCase = restoreBackupUseCase;
            _deleteBackupUseCase = deleteBackupUseCase;
        }

        [HttpGet]
        public async Task<IActionResult> GetBackups(CancellationToken ct)
        {
            var result = await _getBackupsUseCase.ExecuteAsync(ct);
            return Ok(new ApiResponse<IReadOnlyList<AdminBackupResponseDto>> { Success = true, Data = result });
        }

        [HttpPost]
        public async Task<IActionResult> CreateBackup(CancellationToken ct)
        {
            var result = await _createBackupUseCase.ExecuteAsync(ct);
            return StatusCode(StatusCodes.Status202Accepted, new ApiResponse<AdminBackupJobResponseDto>
            {
                Success = true,
                Message = "Backup job started",
                Data = result
            });
        }

        [HttpGet("{id}/download")]
        public async Task<IActionResult> DownloadBackup(string id, CancellationToken ct)
        {
            var result = await _downloadBackupUseCase.ExecuteAsync(id, ct);
            return File(result.Content, result.ContentType, result.FileName);
        }

        [HttpPost("{id}/restore")]
        public async Task<IActionResult> RestoreBackup(string id, RestoreAdminBackupRequestDto request, CancellationToken ct)
        {
            var result = await _restoreBackupUseCase.ExecuteAsync(id, request, ct);
            return StatusCode(StatusCodes.Status202Accepted, new ApiResponse<AdminRestoreJobResponseDto>
            {
                Success = true,
                Message = "Restore job started. System will be unavailable during restore.",
                Data = result
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBackup(string id, CancellationToken ct)
        {
            await _deleteBackupUseCase.ExecuteAsync(id, ct);
            return Ok(new ApiResponse<object> { Success = true, Data = null });
        }
    }
}
