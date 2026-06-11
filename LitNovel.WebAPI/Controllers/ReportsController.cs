using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Report;
using LitNovel.WebAPI.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LitNovel.WebAPI.Controllers
{
    [ApiController]
    [Route("api/reports")]
    public class ReportsController : ControllerBase
    {
        private readonly ICreateUserReportUseCase _createUserReportUseCase;
        private readonly ICreateNovelReportUseCase _createNovelReportUseCase;

        public ReportsController(ICreateUserReportUseCase createUserReportUseCase, ICreateNovelReportUseCase createNovelReportUseCase)
        {
            _createUserReportUseCase = createUserReportUseCase;
            _createNovelReportUseCase = createNovelReportUseCase;
        }

        [HttpPost("users")]
        [Authorize]
        public async Task<IActionResult> CreateUserReport(CreateUserReportRequestDto request, CancellationToken ct)
        {
            var result = await _createUserReportUseCase.ExecuteAsync(request, ct);
            return StatusCode(StatusCodes.Status201Created, new ApiResponse<CreateUserReportResponseDto>
            {
                Success = true,
                Message = "Report submitted",
                Data = result
            });
        }

        [HttpPost("novels")]
        [Authorize]
        public async Task<IActionResult> CreateNovelReport(CreateNovelReportRequestDto request, CancellationToken ct)
        {
            var result = await _createNovelReportUseCase.ExecuteAsync(request, ct);
            return StatusCode(StatusCodes.Status201Created, new ApiResponse<CreateNovelReportResponseDto>
            {
                Success = true,
                Message = "Report submitted",
                Data = result
            });
        }
    }
}
