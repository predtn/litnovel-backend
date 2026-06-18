using LitNovel.Application.DTOs.Staff;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IModerateChapterUseCase
    {
        Task ExecuteAsync(int chapterId, ModerateChapterRequestDto request, CancellationToken ct);
    }
}
