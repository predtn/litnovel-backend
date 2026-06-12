using LitNovel.Application.DTOs.Staff;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IWarnUserUseCase
    {
        Task ExecuteAsync(WarnUserRequestDto request, CancellationToken ct);
    }
}
