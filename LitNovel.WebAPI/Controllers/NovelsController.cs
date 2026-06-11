using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Novel;
using LitNovel.Application.DTOs.Review;
using LitNovel.Application.DTOs.Volume;
using LitNovel.WebAPI.Common;
using LitNovel.WebAPI.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace LitNovel.WebAPI.Controllers
{
    [ApiController]
    [Route("api/novels")]
    public class NovelsController : ControllerBase
    {
        private readonly IGetNovelsUseCase _getNovelsUseCase;
        private readonly IGetNovelUseCase _getNovelUseCase;
        private readonly IGetNovelAnalyticsUseCase _getNovelAnalyticsUseCase;
        private readonly IGetMyNovelsUseCase _getMyNovelsUseCase;
        private readonly ICreateNovelUseCase _createNovelUseCase;
        private readonly IUpdateNovelUseCase _updateNovelUseCase;
        private readonly ISubmitNovelUseCase _submitNovelUseCase;
        private readonly IDeleteNovelUseCase _deleteNovelUseCase;
        private readonly IGetVolumesUseCase _getVolumesUseCase;
        private readonly ICreateVolumeUseCase _createVolumeUseCase;
        private readonly IAddFavoriteUseCase _addFavoriteUseCase;
        private readonly IRemoveFavoriteUseCase _removeFavoriteUseCase;
        private readonly IAddNovelLikeUseCase _addNovelLikeUseCase;
        private readonly IRemoveNovelLikeUseCase _removeNovelLikeUseCase;
        private readonly IGetNovelReviewsUseCase _getNovelReviewsUseCase;
        private readonly ICreateNovelReviewUseCase _createNovelReviewUseCase;

        public NovelsController(
            IGetNovelsUseCase getNovelsUseCase,
            IGetNovelUseCase getNovelUseCase,
            IGetNovelAnalyticsUseCase getNovelAnalyticsUseCase,
            IGetMyNovelsUseCase getMyNovelsUseCase,
            ICreateNovelUseCase createNovelUseCase,
            IUpdateNovelUseCase updateNovelUseCase,
            ISubmitNovelUseCase submitNovelUseCase,
            IDeleteNovelUseCase deleteNovelUseCase,
            IGetVolumesUseCase getVolumesUseCase,
            ICreateVolumeUseCase createVolumeUseCase,
            IAddFavoriteUseCase addFavoriteUseCase,
            IRemoveFavoriteUseCase removeFavoriteUseCase,
            IAddNovelLikeUseCase addNovelLikeUseCase,
            IRemoveNovelLikeUseCase removeNovelLikeUseCase,
            IGetNovelReviewsUseCase getNovelReviewsUseCase,
            ICreateNovelReviewUseCase createNovelReviewUseCase)
        {
            _getNovelsUseCase = getNovelsUseCase;
            _getNovelUseCase = getNovelUseCase;
            _getNovelAnalyticsUseCase = getNovelAnalyticsUseCase;
            _getMyNovelsUseCase = getMyNovelsUseCase;
            _createNovelUseCase = createNovelUseCase;
            _updateNovelUseCase = updateNovelUseCase;
            _submitNovelUseCase = submitNovelUseCase;
            _deleteNovelUseCase = deleteNovelUseCase;
            _getVolumesUseCase = getVolumesUseCase;
            _createVolumeUseCase = createVolumeUseCase;
            _addFavoriteUseCase = addFavoriteUseCase;
            _removeFavoriteUseCase = removeFavoriteUseCase;
            _addNovelLikeUseCase = addNovelLikeUseCase;
            _removeNovelLikeUseCase = removeNovelLikeUseCase;
            _getNovelReviewsUseCase = getNovelReviewsUseCase;
            _createNovelReviewUseCase = createNovelReviewUseCase;
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

        [HttpGet("{id:int}/analytics")]
        [HttpGet("analytics/{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetAnalytics(int id, CancellationToken ct)
        {
            var result = await _getNovelAnalyticsUseCase.ExecuteAsync(id, ct);
            return Ok(new ApiResponse<NovelAnalyticsResponseDto> { Success = true, Data = result });
        }

        [HttpGet("my")]
        [Authorize]
        public async Task<IActionResult> GetMyNovels(ODataQueryOptions<MyNovelListItemResponseDto> queryOptions, CancellationToken ct)
        {
            var result = await ODataQueryResultFactory.ToPagedResultAsync(
                _getMyNovelsUseCase.ExecuteQuery(),
                queryOptions,
                novels => novels.OrderByDescending(n => n.UpdatedAt),
                defaultPageSize: 20,
                maxTop: 100,
                ct);

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
        public async Task<IActionResult> GetVolumes(int novelId, ODataQueryOptions<VolumeResponseDto> queryOptions, CancellationToken ct)
        {
            var result = await ODataQueryResultFactory.ToListAsync(
                await _getVolumesUseCase.ExecuteQueryAsync(novelId, ct),
                queryOptions,
                volumes => volumes.OrderBy(v => v.VolumeNumber),
                defaultTop: 100,
                maxTop: 100,
                ct);

            return Ok(new ApiResponse<List<VolumeResponseDto>> { Success = true, Data = result });
        }

        [HttpPost("{novelId:int}/volumes")]
        [Authorize]
        public async Task<IActionResult> CreateVolume(int novelId, CreateVolumeRequestDto request, CancellationToken ct)
        {
            var result = await _createVolumeUseCase.ExecuteAsync(novelId, request, ct);
            return StatusCode(StatusCodes.Status201Created, new ApiResponse<VolumeResponseDto> { Success = true, Message = "Volume created successfully", Data = result });
        }

        [HttpPost("{id:int}/favorites")]
        [Authorize]
        public async Task<IActionResult> AddFavorite(int id, CancellationToken ct)
        {
            await _addFavoriteUseCase.ExecuteAsync(id, ct);
            return Ok(new ApiResponse<object> { Success = true, Message = "Added to favorites", Data = null });
        }

        [HttpDelete("{id:int}/favorites")]
        [Authorize]
        public async Task<IActionResult> RemoveFavorite(int id, CancellationToken ct)
        {
            await _removeFavoriteUseCase.ExecuteAsync(id, ct);
            return Ok(new ApiResponse<object> { Success = true, Message = "Removed from favorites", Data = null });
        }

        [HttpPost("{id:int}/likes")]
        [Authorize]
        public async Task<IActionResult> AddLike(int id, CancellationToken ct)
        {
            await _addNovelLikeUseCase.ExecuteAsync(id, ct);
            return Ok(new ApiResponse<object> { Success = true, Message = "Novel liked", Data = null });
        }

        [HttpDelete("{id:int}/likes")]
        [Authorize]
        public async Task<IActionResult> RemoveLike(int id, CancellationToken ct)
        {
            await _removeNovelLikeUseCase.ExecuteAsync(id, ct);
            return Ok(new ApiResponse<object> { Success = true, Message = "Novel unliked", Data = null });
        }

        [HttpGet("{id:int}/reviews")]
        public async Task<IActionResult> GetReviews(int id, [FromQuery] int page = 1, [FromQuery] int size = 20, CancellationToken ct = default)
        {
            var result = await _getNovelReviewsUseCase.ExecuteAsync(id, page, size, ct);
            return Ok(new ApiResponse<PagedResult<NovelReviewResponseDto>> { Success = true, Data = result });
        }

        [HttpPost("{id:int}/reviews")]
        [Authorize]
        public async Task<IActionResult> CreateReview(int id, CreateNovelReviewRequestDto request, CancellationToken ct)
        {
            var result = await _createNovelReviewUseCase.ExecuteAsync(id, request, ct);
            return StatusCode(StatusCodes.Status201Created, new ApiResponse<NovelReviewResponseDto> { Success = true, Message = "Review submitted", Data = result });
        }
    }
}
