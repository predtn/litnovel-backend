using LitNovel.Application.DTOs.Tag;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IUpdateAdminTagUseCase
    {
        Task<TagResponseDto> ExecuteAsync(int id, UpdateTagRequestDto request, CancellationToken ct);
    }
}
