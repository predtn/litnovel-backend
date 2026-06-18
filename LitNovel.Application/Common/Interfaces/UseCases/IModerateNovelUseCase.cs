using LitNovel.Application.DTOs.Staff;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IModerateNovelUseCase
    {
        Task ExecuteAsync(int novelId, ModerateNovelRequestDto request, CancellationToken ct);
    }
}
