using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IUpdateAdminNovelStatusUseCase
    {
        Task<AdminNovelStatusResponseDto> ExecuteAsync(int id, UpdateAdminNovelStatusRequestDto request, CancellationToken ct);
    }
}
