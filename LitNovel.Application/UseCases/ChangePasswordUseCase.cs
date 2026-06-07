using FluentValidation;
using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.User;

namespace LitNovel.Application.UseCases
{
    public class ChangePasswordUseCase : IChangePasswordUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IPasswordService _passwordService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<ChangePasswordRequestDto> _validator;

        public ChangePasswordUseCase(IUserRepository userRepository, ICurrentUserService currentUserService, IPasswordService passwordService, IUnitOfWork unitOfWork, IValidator<ChangePasswordRequestDto> validator)
        {
            _userRepository = userRepository;
            _currentUserService = currentUserService;
            _passwordService = passwordService;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task ExecuteAsync(ChangePasswordRequestDto request, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(request, ct);

            var user = await _userRepository.GetByIdAsync(_currentUserService.UserId, ct);
            if (user is null)
            {
                throw new NotFoundException("User not found");
            }

            if (!_passwordService.VerifyPassword(request.CurrentPassword, user.PasswordHash))
            {
                throw new BadRequestException("Current password is incorrect");
            }

            user.PasswordHash = _passwordService.HashPassword(request.NewPassword);
            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
