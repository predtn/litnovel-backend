using FluentValidation;
using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Auth;
using LitNovel.Domain.Entities;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases
{
    public class LoginUseCase : ILoginUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IPasswordService _passwordService;
        private readonly IJwtService _jwtService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<LoginRequestDto> _validator;

        public LoginUseCase(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, IPasswordService passwordService, IJwtService jwtService, IUnitOfWork unitOfWork, IValidator<LoginRequestDto> validator)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _passwordService = passwordService;
            _jwtService = jwtService;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<LoginResponseDto> ExecuteAsync(LoginRequestDto request, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(request, ct);

            var user = await _userRepository.GetByIdentifierAsync(request.Identifier, ct);
            if (user is null || !_passwordService.VerifyPassword(request.Password, user.PasswordHash))
            {
                throw new UnauthorizedException("Invalid credentials");
            }

            if (user.Status == UserStatus.Banned)
            {
                throw new ForbiddenException("Account has been banned");
            }

            user.Status = UserStatus.Online;
            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                Token = _jwtService.GenerateRefreshToken(),
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            await _refreshTokenRepository.AddAsync(refreshToken, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return CreateResponse(user, refreshToken.Token, _jwtService);
        }

        internal static LoginResponseDto CreateResponse(User user, string refreshToken, IJwtService jwtService)
        {
            return new LoginResponseDto
            {
                AccessToken = jwtService.GenerateAccessToken(user),
                RefreshToken = refreshToken,
                ExpiresIn = jwtService.ExpiresInSeconds,
                User = new AuthUserResponseDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    Avatar = user.Avatar,
                    Role = user.Role.ToString(),
                    Status = user.Status.ToString()
                }
            };
        }
    }
}
