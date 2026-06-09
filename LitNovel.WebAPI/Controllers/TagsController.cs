using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Tag;
using LitNovel.WebAPI.Common;
using LitNovel.WebAPI.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace LitNovel.WebAPI.Controllers
{
    [ApiController]
    [Route("api/tags")]
    public class TagsController : ControllerBase
    {
        private readonly IGetTagsUseCase _getTagsUseCase;

        public TagsController(IGetTagsUseCase getTagsUseCase)
        {
            _getTagsUseCase = getTagsUseCase;
        }

        [HttpGet]
        public async Task<IActionResult> Get(ODataQueryOptions<TagResponseDto> queryOptions, CancellationToken ct)
        {
            var result = await _getTagsUseCase.ExecuteAsync(ct);
            result = await ODataQueryResultFactory.ToListAsync(
                result.AsQueryable(),
                queryOptions,
                tags => tags.OrderBy(t => t.Name),
                defaultTop: 100,
                maxTop: 100,
                ct);

            return Ok(new ApiResponse<List<TagResponseDto>> { Success = true, Data = result });
        }
    }
}
