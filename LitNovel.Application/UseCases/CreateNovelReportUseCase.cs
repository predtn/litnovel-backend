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
    public class CreateNovelReportUseCase : ICreateNovelReportUseCase
    {
        private readonly INovelRepository _novelRepository;
        private readonly IChapterRepository _chapterRepository;
        private readonly INovelReportRepository _novelReportRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateNovelReportRequestDto> _validator;

        public CreateNovelReportUseCase(
            INovelRepository novelRepository,
            IChapterRepository chapterRepository,
            INovelReportRepository novelReportRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork,
            IValidator<CreateNovelReportRequestDto> validator)
        {
            _novelRepository = novelRepository;
            _chapterRepository = chapterRepository;
            _novelReportRepository = novelReportRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<CreateNovelReportResponseDto> ExecuteAsync(CreateNovelReportRequestDto request, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(request, ct);

            _ = await _novelRepository.GetByIdWithDetailsAsync(request.TargetNovelId, ct)
                ?? throw new NotFoundException("Novel not found");

            if (request.TargetChapterId.HasValue)
            {
                var chapter = await _chapterRepository.GetByIdWithDetailsAsync(request.TargetChapterId.Value, ct)
                    ?? throw new NotFoundException("Chapter not found");

                if (chapter.Volume.NovelId != request.TargetNovelId)
                {
                    throw new BadRequestException("Chapter does not belong to the reported novel");
                }
            }

            var report = new NovelReport
            {
                ReporterId = _currentUserService.UserId,
                TargetNovelId = request.TargetNovelId,
                TargetChapterId = request.TargetChapterId,
                ReportType = Enum.Parse<ReportType>(request.ReportType, true),
                Description = request.Description
            };

            await _novelReportRepository.AddAsync(report, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return new CreateNovelReportResponseDto
            {
                Id = report.Id,
                TargetNovelId = report.TargetNovelId,
                TargetChapterId = report.TargetChapterId,
                ReportType = report.ReportType.ToString(),
                Status = report.Status.ToString(),
                CreatedAt = report.CreatedAt
            };
        }
    }
}
