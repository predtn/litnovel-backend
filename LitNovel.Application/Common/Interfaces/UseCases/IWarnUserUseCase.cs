using LitNovel.Application.DTOs.Staff;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IWarnUserUseCase
    {
        Task ExecuteAsync(int userId, WarnUserRequestDto request, CancellationToken ct);
    }
}
