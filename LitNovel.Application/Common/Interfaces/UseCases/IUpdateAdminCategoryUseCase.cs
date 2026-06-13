using LitNovel.Application.DTOs.Category;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface IUpdateAdminCategoryUseCase
    {
        Task<CategoryResponseDto> ExecuteAsync(int id, UpdateCategoryRequestDto request, CancellationToken ct);
    }
}
