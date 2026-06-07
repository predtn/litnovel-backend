using FluentValidation;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;
using LitNovel.Application.Common.Models;
using LitNovel.Application.DTOs.Novel;

namespace LitNovel.Application.UseCases
{
    public class GetMyNovelsUseCase : IGetMyNovelsUseCase
    {
        private readonly INovelRepository _novelRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IValidator<MyNovelListQueryDto> _validator;

        public GetMyNovelsUseCase(INovelRepository novelRepository, ICurrentUserService currentUserService, IValidator<MyNovelListQueryDto> validator)
        {
            _novelRepository = novelRepository;
            _currentUserService = currentUserService;
            _validator = validator;
        }

        public async Task<PagedResult<MyNovelListItemResponseDto>> ExecuteAsync(MyNovelListQueryDto query, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(query, ct);
            return await _novelRepository.GetMyNovelsAsync(_currentUserService.UserId, query, ct);
        }
    }
}
