using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.UseCases;

namespace LitNovel.Application.UseCases
{
    public class DeleteAdminBadgeUseCase : IDeleteAdminBadgeUseCase
    {
        private readonly IBadgeRepository _badgeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteAdminBadgeUseCase(IBadgeRepository badgeRepository, IUnitOfWork unitOfWork)
        {
            _badgeRepository = badgeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task ExecuteAsync(int id, CancellationToken ct)
        {
            var badge = await _badgeRepository.GetByIdAsync(id, ct)
                ?? throw new NotFoundException("Badge not found");

            _badgeRepository.Delete(badge);
            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
