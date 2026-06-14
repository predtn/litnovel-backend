using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IUpdateAdminChapterStatusUseCase
    {
        Task<AdminChapterStatusResponseDto> ExecuteAsync(int id, UpdateAdminChapterStatusRequestDto request, CancellationToken ct);
    }
}
