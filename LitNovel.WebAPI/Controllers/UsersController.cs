using LitNovel.Application.Common.Interfaces.UseCases;
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

        public UsersController(IGetMyProfileUseCase getMyProfileUseCase, IUpdateMyProfileUseCase updateMyProfileUseCase, IGetPublicProfileUseCase getPublicProfileUseCase, IChangePasswordUseCase changePasswordUseCase)
        {
            _getMyProfileUseCase = getMyProfileUseCase;
            _updateMyProfileUseCase = updateMyProfileUseCase;
            _getPublicProfileUseCase = getPublicProfileUseCase;
            _changePasswordUseCase = changePasswordUseCase;
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
    }
}
