using FluentValidation;
using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.User;

namespace LitNovel.Application.UseCases
{
    public class UpdateMyProfileUseCase : IUpdateMyProfileUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateMyProfileRequestDto> _validator;

        public UpdateMyProfileUseCase(IUserRepository userRepository, ICurrentUserService currentUserService, IUnitOfWork unitOfWork, IValidator<UpdateMyProfileRequestDto> validator)
        {
            _userRepository = userRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<UpdateMyProfileResponseDto> ExecuteAsync(UpdateMyProfileRequestDto request, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(request, ct);

            var user = await _userRepository.GetByIdAsync(_currentUserService.UserId, ct);
            if (user is null)
            {
                throw new NotFoundException("User not found");
            }

            user.Avatar = request.Avatar;
            user.Bio = request.Bio;
            await _unitOfWork.SaveChangesAsync(ct);

            return new UpdateMyProfileResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Avatar = user.Avatar,
                Bio = user.Bio,
                UpdatedAt = user.UpdatedAt
            };
        }
    }
}
