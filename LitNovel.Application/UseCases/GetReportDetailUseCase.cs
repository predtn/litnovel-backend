using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Staff;

namespace LitNovel.Application.UseCases
{
    public class GetReportDetailUseCase : IGetReportDetailUseCase
    {
        private readonly INovelReportRepository _novelReportRepository;
        private readonly IUserReportRepository _userReportRepository;

        public GetReportDetailUseCase(
            INovelReportRepository novelReportRepository,
            IUserReportRepository userReportRepository)
        {
            _novelReportRepository = novelReportRepository;
            _userReportRepository  = userReportRepository;
        }

        public async Task<ReportDetailResponseDto> ExecuteAsync(int reportId, string kind, CancellationToken ct)
        {
            var normalizedKind = kind?.Trim().ToLowerInvariant();

            if (normalizedKind == "novel")
            {
                var report = await _novelReportRepository.GetByIdWithDetailsAsync(reportId, ct)
                    ?? throw new NotFoundException("Report not found.");

                return new ReportDetailResponseDto
                {
                    Id              = report.Id,
                    Kind            = "Novel",
                    ReportType      = report.ReportType.ToString(),
                    Description     = report.Description,
                    Status          = report.Status.ToString(),
                    ActionTaken     = report.ActionTaken,
                    ResolutionNotes = report.ResolutionNotes,
                    CreatedAt       = report.CreatedAt,
                    UpdatedAt       = report.UpdatedAt,
                    Reporter        = report.Reporter == null ? null : new ReportActorDto
                    {
                        Id = report.Reporter.Id, Username = report.Reporter.Username, Avatar = report.Reporter.Avatar
                    },
                    ProcessedBy     = report.ProcessedBy == null ? null : new ReportActorDto
                    {
                        Id = report.ProcessedBy.Id, Username = report.ProcessedBy.Username, Avatar = report.ProcessedBy.Avatar
                    },
                    TargetNovel     = report.TargetNovel == null ? null : new ReportTargetNovelDto
                    {
                        Id = report.TargetNovel.Id, Title = report.TargetNovel.Title, Slug = report.TargetNovel.Slug
                    },
                    TargetChapter   = report.TargetChapter == null ? null : new ReportTargetChapterDto
                    {
                        Id = report.TargetChapter.Id, Title = report.TargetChapter.Title, ChapterNumber = report.TargetChapter.ChapterNumber
                    }
                };
            }

            if (normalizedKind == "user")
            {
                var report = await _userReportRepository.GetByIdWithDetailsAsync(reportId, ct)
                    ?? throw new NotFoundException("Report not found.");

                return new ReportDetailResponseDto
                {
                    Id              = report.Id,
                    Kind            = "User",
                    ReportType      = report.ReportType.ToString(),
                    Description     = report.Description,
                    Status          = report.Status.ToString(),
                    ActionTaken     = report.ActionTaken,
                    ResolutionNotes = report.ResolutionNotes,
                    CreatedAt       = report.CreatedAt,
                    UpdatedAt       = report.UpdatedAt,
                    Reporter        = report.Reporter == null ? null : new ReportActorDto
                    {
                        Id = report.Reporter.Id, Username = report.Reporter.Username, Avatar = report.Reporter.Avatar
                    },
                    ProcessedBy     = report.ProcessedBy == null ? null : new ReportActorDto
                    {
                        Id = report.ProcessedBy.Id, Username = report.ProcessedBy.Username, Avatar = report.ProcessedBy.Avatar
                    },
                    TargetUser      = report.TargetUser == null ? null : new ReportActorDto
                    {
                        Id = report.TargetUser.Id, Username = report.TargetUser.Username, Avatar = report.TargetUser.Avatar
                    }
                };
            }

            throw new BadRequestException("Kind must be 'Novel' or 'User'.");
        }
    }
}
