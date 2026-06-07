using LitNovel.Application.DTOs.Novel;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface ICreateNovelUseCase
    {
        Task<CreateNovelResponseDto> ExecuteAsync(CreateNovelRequestDto request, CancellationToken ct);
    }
}
