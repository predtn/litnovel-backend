using FluentValidation;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Auth;

namespace LitNovel.Application.UseCases
{
    public class ForgotPasswordUseCase : IForgotPasswordUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordResetTokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly IValidator<ForgotPasswordRequestDto> _validator;

        public ForgotPasswordUseCase(IUserRepository userRepository, IPasswordResetTokenService tokenService, IEmailService emailService, IValidator<ForgotPasswordRequestDto> validator)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _emailService = emailService;
            _validator = validator;
        }

        public async Task ExecuteAsync(ForgotPasswordRequestDto request, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(request, ct);

            var user = await _userRepository.GetByIdentifierAsync(request.Email, ct);
            if (user is null)
            {
                return;
            }

            var token = _tokenService.GenerateToken(user.Id, user.PasswordHash);
            await _emailService.SendPasswordResetAsync(user.Email, token, ct);
        }
    }
}
