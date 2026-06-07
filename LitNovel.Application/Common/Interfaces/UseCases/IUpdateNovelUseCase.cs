using LitNovel.Application.DTOs.Novel;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IUpdateNovelUseCase
    {
        Task<UpdateNovelResponseDto> ExecuteAsync(int id, UpdateNovelRequestDto request, CancellationToken ct);
    }
}
