using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Auth;
using LitNovel.WebAPI.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace LitNovel.WebAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IRegisterUseCase _registerUseCase;
        private readonly ILoginUseCase _loginUseCase;
        private readonly IRefreshTokenUseCase _refreshTokenUseCase;
        private readonly ILogoutUseCase _logoutUseCase;
        private readonly IForgotPasswordUseCase _forgotPasswordUseCase;
        private readonly IResetPasswordUseCase _resetPasswordUseCase;

        public AuthController(IRegisterUseCase registerUseCase, ILoginUseCase loginUseCase, IRefreshTokenUseCase refreshTokenUseCase, ILogoutUseCase logoutUseCase, IForgotPasswordUseCase forgotPasswordUseCase, IResetPasswordUseCase resetPasswordUseCase)
        {
            _registerUseCase = registerUseCase;
            _loginUseCase = loginUseCase;
            _refreshTokenUseCase = refreshTokenUseCase;
            _logoutUseCase = logoutUseCase;
            _forgotPasswordUseCase = forgotPasswordUseCase;
            _resetPasswordUseCase = resetPasswordUseCase;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto request, CancellationToken ct)
        {
            var result = await _registerUseCase.ExecuteAsync(request, ct);
            return StatusCode(StatusCodes.Status201Created, new ApiResponse<RegisterResponseDto>
            {
                Success = true,
                Message = "Registration successful",
                Data = result
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto request, CancellationToken ct)
        {
            var result = await _loginUseCase.ExecuteAsync(request, ct);
            return Ok(new ApiResponse<LoginResponseDto>
            {
                Success = true,
                Message = "Login successful",
                Data = result
            });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshTokenRequestDto request, CancellationToken ct)
        {
            var result = await _refreshTokenUseCase.ExecuteAsync(request, ct);
            return Ok(new ApiResponse<LoginResponseDto>
            {
                Success = true,
                Message = "Token refreshed",
                Data = result
            });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(LogoutRequestDto request, CancellationToken ct)
        {
            await _logoutUseCase.ExecuteAsync(request, ct);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Logout successful",
                Data = null
            });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequestDto request, CancellationToken ct)
        {
            await _forgotPasswordUseCase.ExecuteAsync(request, ct);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "If this email is registered, you will receive a reset link shortly",
                Data = null
            });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequestDto request, CancellationToken ct)
        {
            await _resetPasswordUseCase.ExecuteAsync(request, ct);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Password reset successfully. Please login.",
                Data = null
            });
        }
    }
}
