using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;

namespace LitNovel.Application.UseCases
{
    public class IncrementNovelViewUseCase : IIncrementNovelViewUseCase
    {
        private readonly INovelRepository _novelRepository;

        public IncrementNovelViewUseCase(INovelRepository novelRepository)
        {
            _novelRepository = novelRepository;
        }

        public async Task ExecuteAsync(int id, CancellationToken ct)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Invalid novel id");
            }

            var affected = await _novelRepository.IncrementViewCountAsync(id, ct);
            if (affected == 0)
            {
                throw new NotFoundException("Novel not found");
            }
        }
    }
}
