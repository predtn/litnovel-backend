using FluentValidation;
using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Chapter;
using LitNovel.Domain.Entities;

namespace LitNovel.Application.UseCases
{
    public class CreateChapterUseCase : ICreateChapterUseCase
    {
        private readonly IVolumeRepository _volumeRepository;
        private readonly IChapterRepository _chapterRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateChapterRequestDto> _validator;

        public CreateChapterUseCase(
            IVolumeRepository volumeRepository,
            IChapterRepository chapterRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork,
            IValidator<CreateChapterRequestDto> validator)
        {
            _volumeRepository = volumeRepository;
            _chapterRepository = chapterRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<ChapterResponseDto> ExecuteAsync(int volumeId, CreateChapterRequestDto request, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(request, ct);
            if (volumeId <= 0)
            {
                throw new BadRequestException("Invalid volume id");
            }

            var volume = await _volumeRepository.GetByIdForUpdateAsync(volumeId, ct);
            if (volume == null)
            {
                throw new NotFoundException("Volume not found");
            }

            if (!VolumePermissionHelper.CanManage(_currentUserService, volume.Novel.AuthorId))
            {
                throw new ForbiddenException("You do not have permission to edit this novel");
            }

            if (await _chapterRepository.ChapterNumberExistsAsync(volumeId, request.ChapterNumber, null, ct))
            {
                throw new ConflictException("Chapter number already exists in this volume");
            }

            var chapter = new Chapter
            {
                VolumeId = volumeId,
                ChapterNumber = request.ChapterNumber,
                Title = request.Title.Trim(),
                Slug = NovelSlugGenerator.Generate($"{volumeId}-{request.ChapterNumber}-{request.Title}"),
                ReleaseDate = request.ReleaseDate,
                Content = new ChapterContent
                {
                    Content = request.Content.Trim()
                }
            };

            volume.Novel.TotalChapters++;
            await _chapterRepository.AddAsync(chapter, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return new ChapterResponseDto
            {
                Id = chapter.Id,
                ChapterNumber = chapter.ChapterNumber,
                Title = chapter.Title,
                Status = chapter.Status.ToString(),
                VolumeId = chapter.VolumeId,
                CreatedAt = chapter.CreatedAt
            };
        }
    }
}
