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

        public ReportsController(ICreateUserReportUseCase createUserReportUseCase)
        {
            _createUserReportUseCase = createUserReportUseCase;
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
    }
}
