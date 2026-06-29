using FluentValidation;
using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Auth;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases
{
    public class LogoutUseCase : ILogoutUseCase
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<LogoutRequestDto> _validator;

        public LogoutUseCase(IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork, IValidator<LogoutRequestDto> validator)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task ExecuteAsync(LogoutRequestDto request, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(request, ct);

            var refreshToken = await _refreshTokenRepository.GetActiveAsync(request.RefreshToken, ct);
            if (refreshToken is null)
            {
                throw new UnauthorizedException("Invalid refresh token");
            }

            _refreshTokenRepository.Revoke(refreshToken);
            refreshToken.User.Status = UserStatus.Offline;

            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
