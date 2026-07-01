using LitNovel.Application.DTOs.Staff;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IUpdateUserStatusUseCase
    {
        Task ExecuteAsync(int userId, UpdateUserStatusRequestDto request, CancellationToken ct);
    }
}
