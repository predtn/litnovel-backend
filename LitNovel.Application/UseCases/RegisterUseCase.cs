using FluentValidation;
using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Auth;
using LitNovel.Domain.Entities;

namespace LitNovel.Application.UseCases
{
    public class RegisterUseCase : IRegisterUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<RegisterRequestDto> _validator;

        public RegisterUseCase(IUserRepository userRepository, IPasswordService passwordService, IUnitOfWork unitOfWork, IValidator<RegisterRequestDto> validator)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<RegisterResponseDto> ExecuteAsync(RegisterRequestDto request, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(request, ct);

            if (await _userRepository.EmailExistsAsync(request.Email, ct))
            {
                throw new ConflictException("Email already exists");
            }

            if (await _userRepository.UsernameExistsAsync(request.Username, ct))
            {
                throw new ConflictException("Username already taken");
            }

            var user = new User
            {
                Username = request.Username.Trim(),
                Email = request.Email.Trim(),
                PasswordHash = _passwordService.HashPassword(request.Password)
            };

            await _userRepository.AddAsync(user, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return new RegisterResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role.ToString(),
                CreatedAt = user.CreatedAt
            };
        }
    }
}
