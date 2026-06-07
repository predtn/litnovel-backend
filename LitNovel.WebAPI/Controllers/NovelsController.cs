using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Novel;
using LitNovel.Application.DTOs.Volume;
using LitNovel.WebAPI.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LitNovel.WebAPI.Controllers
{
    [ApiController]
    [Route("api/novels")]
    public class NovelsController : ControllerBase
    {
        private readonly IGetNovelsUseCase _getNovelsUseCase;
        private readonly IGetNovelUseCase _getNovelUseCase;
        private readonly IGetMyNovelsUseCase _getMyNovelsUseCase;
        private readonly ICreateNovelUseCase _createNovelUseCase;
        private readonly IUpdateNovelUseCase _updateNovelUseCase;
        private readonly ISubmitNovelUseCase _submitNovelUseCase;
        private readonly IDeleteNovelUseCase _deleteNovelUseCase;
        private readonly IGetVolumesUseCase _getVolumesUseCase;
        private readonly ICreateVolumeUseCase _createVolumeUseCase;

        public NovelsController(
            IGetNovelsUseCase getNovelsUseCase,
            IGetNovelUseCase getNovelUseCase,
            IGetMyNovelsUseCase getMyNovelsUseCase,
            ICreateNovelUseCase createNovelUseCase,
            IUpdateNovelUseCase updateNovelUseCase,
            ISubmitNovelUseCase submitNovelUseCase,
            IDeleteNovelUseCase deleteNovelUseCase,
            IGetVolumesUseCase getVolumesUseCase,
            ICreateVolumeUseCase createVolumeUseCase)
        {
            _getNovelsUseCase = getNovelsUseCase;
            _getNovelUseCase = getNovelUseCase;
            _getMyNovelsUseCase = getMyNovelsUseCase;
            _createNovelUseCase = createNovelUseCase;
            _updateNovelUseCase = updateNovelUseCase;
            _submitNovelUseCase = submitNovelUseCase;
            _deleteNovelUseCase = deleteNovelUseCase;
            _getVolumesUseCase = getVolumesUseCase;
            _createVolumeUseCase = createVolumeUseCase;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] NovelListQueryDto query, CancellationToken ct)
        {
            var result = await _getNovelsUseCase.ExecuteAsync(query, ct);
            return Ok(new ApiResponse<PagedResult<NovelListItemResponseDto>> { Success = true, Data = result });
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var result = await _getNovelUseCase.ExecuteAsync(id, ct);
            return Ok(new ApiResponse<NovelDetailResponseDto> { Success = true, Data = result });
        }

        [HttpGet("my")]
        [Authorize]
        public async Task<IActionResult> GetMyNovels([FromQuery] MyNovelListQueryDto query, CancellationToken ct)
        {
            var result = await _getMyNovelsUseCase.ExecuteAsync(query, ct);
            return Ok(new ApiResponse<PagedResult<MyNovelListItemResponseDto>> { Success = true, Data = result });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateNovelRequestDto request, CancellationToken ct)
        {
            var result = await _createNovelUseCase.ExecuteAsync(request, ct);
            return StatusCode(StatusCodes.Status201Created, new ApiResponse<CreateNovelResponseDto> { Success = true, Message = "Novel created successfully", Data = result });
        }

        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, UpdateNovelRequestDto request, CancellationToken ct)
        {
            var result = await _updateNovelUseCase.ExecuteAsync(id, request, ct);
            return Ok(new ApiResponse<UpdateNovelResponseDto> { Success = true, Message = "Novel updated successfully", Data = result });
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            await _deleteNovelUseCase.ExecuteAsync(id, ct);
            return Ok(new ApiResponse<object> { Success = true, Data = null });
        }

        [HttpPost("{id:int}/submit")]
        [Authorize]
        public async Task<IActionResult> Submit(int id, CancellationToken ct)
        {
            var result = await _submitNovelUseCase.ExecuteAsync(id, ct);
            return Ok(new ApiResponse<SubmitNovelResponseDto> { Success = true, Message = "Novel submitted for review", Data = result });
        }

        [HttpGet("{novelId:int}/volumes")]
        [Authorize]
        public async Task<IActionResult> GetVolumes(int novelId, CancellationToken ct)
        {
            var result = await _getVolumesUseCase.ExecuteAsync(novelId, ct);
            return Ok(new ApiResponse<List<VolumeResponseDto>> { Success = true, Data = result });
        }

        [HttpPost("{novelId:int}/volumes")]
        [Authorize]
        public async Task<IActionResult> CreateVolume(int novelId, CreateVolumeRequestDto request, CancellationToken ct)
        {
            var result = await _createVolumeUseCase.ExecuteAsync(novelId, request, ct);
            return StatusCode(StatusCodes.Status201Created, new ApiResponse<VolumeResponseDto> { Success = true, Message = "Volume created successfully", Data = result });
        }
    }
}
