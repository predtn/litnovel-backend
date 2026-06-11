using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Novel;
using LitNovel.Application.DTOs.Reading;
using LitNovel.Application.DTOs.User;
using LitNovel.WebAPI.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LitNovel.WebAPI.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IGetMyProfileUseCase _getMyProfileUseCase;
        private readonly IUpdateMyProfileUseCase _updateMyProfileUseCase;
        private readonly IGetPublicProfileUseCase _getPublicProfileUseCase;
        private readonly IChangePasswordUseCase _changePasswordUseCase;
        private readonly ISearchUsersUseCase _searchUsersUseCase;
        private readonly IGetReadingHistoryUseCase _getReadingHistoryUseCase;
        private readonly IDeleteReadingHistoryUseCase _deleteReadingHistoryUseCase;
        private readonly IGetMyFavoritesUseCase _getMyFavoritesUseCase;
        private readonly IGetMyNovelLikesUseCase _getMyNovelLikesUseCase;

        public UsersController(
            IGetMyProfileUseCase getMyProfileUseCase,
            IUpdateMyProfileUseCase updateMyProfileUseCase,
            IGetPublicProfileUseCase getPublicProfileUseCase,
            IChangePasswordUseCase changePasswordUseCase,
            ISearchUsersUseCase searchUsersUseCase,
            IGetReadingHistoryUseCase getReadingHistoryUseCase,
            IDeleteReadingHistoryUseCase deleteReadingHistoryUseCase,
            IGetMyFavoritesUseCase getMyFavoritesUseCase,
            IGetMyNovelLikesUseCase getMyNovelLikesUseCase)
        {
            _getMyProfileUseCase = getMyProfileUseCase;
            _updateMyProfileUseCase = updateMyProfileUseCase;
            _getPublicProfileUseCase = getPublicProfileUseCase;
            _changePasswordUseCase = changePasswordUseCase;
            _searchUsersUseCase = searchUsersUseCase;
            _getReadingHistoryUseCase = getReadingHistoryUseCase;
            _deleteReadingHistoryUseCase = deleteReadingHistoryUseCase;
            _getMyFavoritesUseCase = getMyFavoritesUseCase;
            _getMyNovelLikesUseCase = getMyNovelLikesUseCase;
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetMe(CancellationToken ct)
        {
            var result = await _getMyProfileUseCase.ExecuteAsync(ct);
            return Ok(new ApiResponse<MyProfileResponseDto> { Success = true, Data = result });
        }

        [HttpPut("me")]
        [Authorize]
        public async Task<IActionResult> UpdateMe(UpdateMyProfileRequestDto request, CancellationToken ct)
        {
            var result = await _updateMyProfileUseCase.ExecuteAsync(request, ct);
            return Ok(new ApiResponse<UpdateMyProfileResponseDto>
            {
                Success = true,
                Message = "Profile updated successfully",
                Data = result
            });
        }

        [HttpPut("me/password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequestDto request, CancellationToken ct)
        {
            await _changePasswordUseCase.ExecuteAsync(request, ct);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Password changed successfully",
                Data = null
            });
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetPublicProfile(int id, CancellationToken ct)
        {
            var result = await _getPublicProfileUseCase.ExecuteAsync(id, ct);
            return Ok(new ApiResponse<PublicProfileResponseDto> { Success = true, Data = result });
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] UserSearchQueryDto query, CancellationToken ct)
        {
            var result = await _searchUsersUseCase.ExecuteAsync(query, ct);
            return Ok(new ApiResponse<PagedResult<UserSearchResponseDto>> { Success = true, Data = result });
        }

        [HttpGet("me/reading-history")]
        [Authorize]
        public async Task<IActionResult> GetReadingHistory([FromQuery] ReadingHistoryQueryDto query, CancellationToken ct)
        {
            var result = await _getReadingHistoryUseCase.ExecuteAsync(query, ct);
            return Ok(new ApiResponse<PagedResult<ReadingProgressResponseDto>> { Success = true, Data = result });
        }

        [HttpDelete("me/reading-history/{novelId:int}")]
        [Authorize]
        public async Task<IActionResult> DeleteReadingHistory(int novelId, CancellationToken ct)
        {
            await _deleteReadingHistoryUseCase.ExecuteAsync(novelId, ct);
            return Ok(new ApiResponse<object> { Success = true, Data = null });
        }

        [HttpGet("me/favorites")]
        [Authorize]
        public async Task<IActionResult> GetFavorites([FromQuery] int page = 1, [FromQuery] int size = 20, CancellationToken ct = default)
        {
            var result = await _getMyFavoritesUseCase.ExecuteAsync(page, size, ct);
            return Ok(new ApiResponse<PagedResult<NovelListItemResponseDto>> { Success = true, Data = result });
        }

        [HttpGet("me/bookmarks")]
        [Authorize]
        public async Task<IActionResult> GetBookmarks([FromQuery] int page = 1, [FromQuery] int size = 20, CancellationToken ct = default)
        {
            var result = await _getMyFavoritesUseCase.ExecuteAsync(page, size, ct);
            return Ok(new ApiResponse<PagedResult<NovelListItemResponseDto>> { Success = true, Data = result });
        }

        [HttpGet("me/likes")]
        [Authorize]
        public async Task<IActionResult> GetLikes([FromQuery] int page = 1, [FromQuery] int size = 20, CancellationToken ct = default)
        {
            var result = await _getMyNovelLikesUseCase.ExecuteAsync(page, size, ct);
            return Ok(new ApiResponse<PagedResult<NovelListItemResponseDto>> { Success = true, Data = result });
        }
    }
}
