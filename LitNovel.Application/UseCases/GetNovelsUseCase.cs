using FluentValidation;
using LitNovel.Application.Common.Models;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.DTOs.Novel;

namespace LitNovel.Application.UseCases
{
    public class GetNovelsUseCase : IGetNovelsUseCase
    {
        private readonly INovelRepository _novelRepository;
        private readonly IValidator<NovelListQueryDto> _validator;

        public GetNovelsUseCase(INovelRepository novelRepository, IValidator<NovelListQueryDto> validator)
        {
            _novelRepository = novelRepository;
            _validator = validator;
        }

        public async Task<PagedResult<NovelListItemResponseDto>> ExecuteAsync(NovelListQueryDto query, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(query, ct);
            return await _novelRepository.GetListAsync(query, ct);
        }
    }
}
