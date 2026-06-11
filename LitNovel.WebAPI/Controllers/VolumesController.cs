using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Chapter;
using LitNovel.Application.DTOs.Volume;
using LitNovel.WebAPI.Common;
using LitNovel.WebAPI.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace LitNovel.WebAPI.Controllers
{
    [ApiController]
    [Route("api/volumes")]
    [Authorize]
    public class VolumesController : ControllerBase
    {
        private readonly IUpdateVolumeUseCase _updateVolumeUseCase;
        private readonly IDeleteVolumeUseCase _deleteVolumeUseCase;
        private readonly IGetChaptersUseCase _getChaptersUseCase;
        private readonly ICreateChapterUseCase _createChapterUseCase;

        public VolumesController(
            IUpdateVolumeUseCase updateVolumeUseCase,
            IDeleteVolumeUseCase deleteVolumeUseCase,
            IGetChaptersUseCase getChaptersUseCase,
            ICreateChapterUseCase createChapterUseCase)
        {
            _updateVolumeUseCase = updateVolumeUseCase;
            _deleteVolumeUseCase = deleteVolumeUseCase;
            _getChaptersUseCase = getChaptersUseCase;
            _createChapterUseCase = createChapterUseCase;
        }

        [HttpPut("{volumeId:int}")]
        public async Task<IActionResult> Update(int volumeId, UpdateVolumeRequestDto request, CancellationToken ct)
        {
            var result = await _updateVolumeUseCase.ExecuteAsync(volumeId, request, ct);
            return Ok(new ApiResponse<VolumeResponseDto> { Success = true, Message = "Volume updated successfully", Data = result });
        }

        [HttpDelete("{volumeId:int}")]
        public async Task<IActionResult> Delete(int volumeId, CancellationToken ct)
        {
            await _deleteVolumeUseCase.ExecuteAsync(volumeId, ct);
            return Ok(new ApiResponse<object> { Success = true, Data = null });
        }

        [HttpGet("{volumeId:int}/chapters")]
        public async Task<IActionResult> GetChapters(int volumeId, ODataQueryOptions<ChapterListItemResponseDto> queryOptions, CancellationToken ct)
        {
            var result = await ODataQueryResultFactory.ToPagedResultAsync(
                await _getChaptersUseCase.ExecuteQueryAsync(volumeId, ct),
                queryOptions,
                chapters => chapters.OrderBy(c => c.ChapterNumber),
                defaultPageSize: 50,
                maxTop: 100,
                ct);

            return Ok(new ApiResponse<PagedResult<ChapterListItemResponseDto>> { Success = true, Data = result });
        }

        [HttpPost("{volumeId:int}/chapters")]
        public async Task<IActionResult> CreateChapter(int volumeId, [FromBody] CreateChapterRequestDto request, CancellationToken ct)
        {
            var result = await _createChapterUseCase.ExecuteAsync(volumeId, request, ct);
            return StatusCode(StatusCodes.Status201Created, new ApiResponse<ChapterResponseDto> { Success = true, Message = "Chapter created successfully", Data = result });
        }
    }
}
