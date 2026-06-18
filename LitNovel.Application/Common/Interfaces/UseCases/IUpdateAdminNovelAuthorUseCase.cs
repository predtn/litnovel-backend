using LitNovel.Application.DTOs.Admin;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IUpdateAdminNovelAuthorUseCase
    {
        Task<AdminNovelAuthorResponseDto> ExecuteAsync(int id, UpdateAdminNovelAuthorRequestDto request, CancellationToken ct);
    }
}
