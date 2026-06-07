using LitNovel.Application.DTOs.Novel;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface ISubmitNovelUseCase
    {
        Task<SubmitNovelResponseDto> ExecuteAsync(int id, CancellationToken ct);
    }
}
