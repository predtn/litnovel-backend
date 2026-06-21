using LitNovel.Application.DTOs.Tag;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface ICreateAdminTagUseCase
    {
        Task<TagResponseDto> ExecuteAsync(CreateTagRequestDto request, CancellationToken ct);
    }
}
