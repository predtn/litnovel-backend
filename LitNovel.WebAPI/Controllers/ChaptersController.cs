using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Chapter;
using LitNovel.WebAPI.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LitNovel.WebAPI.Controllers
{
    [ApiController]
    [Route("api/chapters")]
    [Authorize]
    public class ChaptersController : ControllerBase
    {
        private readonly IGetChapterUseCase _getChapterUseCase;
        private readonly IUpdateChapterUseCase _updateChapterUseCase;
        private readonly ISubmitChapterUseCase _submitChapterUseCase;
        private readonly IDeleteChapterUseCase _deleteChapterUseCase;

        public ChaptersController(
            IGetChapterUseCase getChapterUseCase,
            IUpdateChapterUseCase updateChapterUseCase,
            ISubmitChapterUseCase submitChapterUseCase,
            IDeleteChapterUseCase deleteChapterUseCase)
        {
            _getChapterUseCase = getChapterUseCase;
            _updateChapterUseCase = updateChapterUseCase;
            _submitChapterUseCase = submitChapterUseCase;
            _deleteChapterUseCase = deleteChapterUseCase;
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var result = await _getChapterUseCase.ExecuteAsync(id, ct);
            return Ok(new ApiResponse<ChapterDetailResponseDto> { Success = true, Data = result });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateChapterRequestDto request, CancellationToken ct)
        {
            var result = await _updateChapterUseCase.ExecuteAsync(id, request, ct);
            return Ok(new ApiResponse<UpdateChapterResponseDto> { Success = true, Message = "Chapter updated successfully", Data = result });
        }

        [HttpPost("{id:int}/submit")]
        public async Task<IActionResult> Submit(int id, CancellationToken ct)
        {
            var result = await _submitChapterUseCase.ExecuteAsync(id, ct);
            return Ok(new ApiResponse<SubmitChapterResponseDto> { Success = true, Message = "Chapter submitted for review", Data = result });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            await _deleteChapterUseCase.ExecuteAsync(id, ct);
            return Ok(new ApiResponse<object> { Success = true, Data = null });
        }
    }
}
