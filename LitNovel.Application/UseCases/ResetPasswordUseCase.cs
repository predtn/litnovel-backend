using FluentValidation;
using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Auth;

namespace LitNovel.Application.UseCases
{
    public class ResetPasswordUseCase : IResetPasswordUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IPasswordResetTokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<ResetPasswordRequestDto> _validator;

        public ResetPasswordUseCase(IUserRepository userRepository, IPasswordService passwordService, IPasswordResetTokenService tokenService, IUnitOfWork unitOfWork, IValidator<ResetPasswordRequestDto> validator)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task ExecuteAsync(ResetPasswordRequestDto request, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(request, ct);

            if (!_tokenService.TryValidateToken(request.Token, string.Empty, out var userId))
            {
                throw new BadRequestException("Invalid or expired reset token");
            }

            var user = await _userRepository.GetByIdAsync(userId, ct);
            if (user is null || !_tokenService.TryValidateToken(request.Token, user.PasswordHash, out _))
            {
                throw new BadRequestException("Invalid or expired reset token");
            }

            user.PasswordHash = _passwordService.HashPassword(request.NewPassword);
            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
