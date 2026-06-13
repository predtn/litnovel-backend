using LitNovel.Application.DTOs.Category;

namespace LitNovel.Application.Common.Interfaces.UseCases
{
    public interface ICreateAdminCategoryUseCase
    {
        Task<CategoryResponseDto> ExecuteAsync(CreateCategoryRequestDto request, CancellationToken ct);
    }
}
