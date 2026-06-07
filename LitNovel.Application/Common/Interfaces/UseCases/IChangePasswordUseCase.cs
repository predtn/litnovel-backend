using LitNovel.Application.DTOs.User;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IChangePasswordUseCase
    {
        Task ExecuteAsync(ChangePasswordRequestDto request, CancellationToken ct);
    }
}
