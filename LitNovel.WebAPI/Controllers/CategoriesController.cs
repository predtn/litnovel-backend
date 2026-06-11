using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Category;
using LitNovel.WebAPI.Common;
using LitNovel.WebAPI.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

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
        public async Task<IActionResult> Get(ODataQueryOptions<CategoryResponseDto> queryOptions, CancellationToken ct)
        {
            var result = await _getCategoriesUseCase.ExecuteAsync(ct);
            result = await ODataQueryResultFactory.ToListAsync(
                result.AsQueryable(),
                queryOptions,
                categories => categories.OrderBy(c => c.Name),
                defaultTop: 100,
                maxTop: 100,
                ct);

            return Ok(new ApiResponse<List<CategoryResponseDto>> { Success = true, Data = result });
        }
    }
}
