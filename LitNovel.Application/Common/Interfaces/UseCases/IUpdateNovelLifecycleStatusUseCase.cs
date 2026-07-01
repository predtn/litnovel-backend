using LitNovel.Application.DTOs.Novel;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IUpdateNovelLifecycleStatusUseCase
    {
        Task<UpdateNovelResponseDto> ExecuteAsync(int id, UpdateNovelLifecycleStatusRequestDto request, CancellationToken ct);
    }
}
