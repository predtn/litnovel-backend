using LitNovel.Application.DTOs.Novel;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IRestoreNovelUseCase
    {
        Task<UpdateNovelResponseDto> ExecuteAsync(int id, CancellationToken ct);
    }
}
