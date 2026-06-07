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
    public class RefreshTokenUseCase : IRefreshTokenUseCase
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IJwtService _jwtService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<RefreshTokenRequestDto> _validator;

        public RefreshTokenUseCase(IRefreshTokenRepository refreshTokenRepository, IJwtService jwtService, IUnitOfWork unitOfWork, IValidator<RefreshTokenRequestDto> validator)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _jwtService = jwtService;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<LoginResponseDto> ExecuteAsync(RefreshTokenRequestDto request, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(request, ct);

            var existing = await _refreshTokenRepository.GetActiveAsync(request.RefreshToken, ct);
            if (existing is null)
            {
                throw new UnauthorizedException("Invalid refresh token");
            }

            if (existing.User.Status == UserStatus.Banned)
            {
                throw new ForbiddenException("Account has been banned");
            }

            _refreshTokenRepository.Revoke(existing);
            var refreshToken = new RefreshToken
            {
                UserId = existing.UserId,
                Token = _jwtService.GenerateRefreshToken(),
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            await _refreshTokenRepository.AddAsync(refreshToken, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return LoginUseCase.CreateResponse(existing.User, refreshToken.Token, _jwtService);
        }
    }
}
