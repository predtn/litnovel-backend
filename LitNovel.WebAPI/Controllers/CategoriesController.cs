using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Category;
using LitNovel.WebAPI.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace LitNovel.WebAPI.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly IGetCategoriesUseCase _getCategoriesUseCase;

        public CategoriesController(IGetCategoriesUseCase getCategoriesUseCase)
        {
            _getCategoriesUseCase = getCategoriesUseCase;
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken ct)
        {
            var result = await _getCategoriesUseCase.ExecuteAsync(ct);
            return Ok(new ApiResponse<List<CategoryResponseDto>> { Success = true, Data = result });
        }
    }
}
