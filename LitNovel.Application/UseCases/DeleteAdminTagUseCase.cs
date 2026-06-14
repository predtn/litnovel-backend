using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;

namespace LitNovel.Application.UseCases
{
    public class DeleteAdminTagUseCase : IDeleteAdminTagUseCase
    {
        private readonly ITagRepository _tagRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteAdminTagUseCase(ITagRepository tagRepository, IUnitOfWork unitOfWork)
        {
            _tagRepository = tagRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task ExecuteAsync(int id, CancellationToken ct)
        {
            var tag = await _tagRepository.GetByIdAsync(id, ct)
                ?? throw new NotFoundException("Tag not found");

            _tagRepository.Delete(tag);
            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
