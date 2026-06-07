using FluentValidation;
using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Report;
using LitNovel.Domain.Entities;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases
{
    public class CreateUserReportUseCase : ICreateUserReportUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserReportRepository _userReportRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateUserReportRequestDto> _validator;

        public CreateUserReportUseCase(IUserRepository userRepository, IUserReportRepository userReportRepository, ICurrentUserService currentUserService, IUnitOfWork unitOfWork, IValidator<CreateUserReportRequestDto> validator)
        {
            _userRepository = userRepository;
            _userReportRepository = userReportRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<CreateUserReportResponseDto> ExecuteAsync(CreateUserReportRequestDto request, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(request, ct);

            if (await _userRepository.GetByIdAsync(request.TargetUserId, ct) is null)
            {
                throw new NotFoundException("User not found");
            }

            var report = new UserReport
            {
                ReporterId = _currentUserService.UserId,
                TargetUserId = request.TargetUserId,
                TargetCommentId = request.TargetCommentId,
                ReportType = Enum.Parse<ReportType>(request.ReportType, true),
                Description = request.Description
            };

            await _userReportRepository.AddAsync(report, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return new CreateUserReportResponseDto
            {
                Id = report.Id,
                TargetUserId = report.TargetUserId,
                ReportType = report.ReportType.ToString(),
                Status = report.Status.ToString(),
                CreatedAt = report.CreatedAt
            };
        }
    }
}
