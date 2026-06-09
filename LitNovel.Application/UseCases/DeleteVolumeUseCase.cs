using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Repositories;
using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Application.Common.Interfaces.UseCases;

namespace LitNovel.Application.UseCases
{
    public class DeleteVolumeUseCase : IDeleteVolumeUseCase
    {
        private readonly IVolumeRepository _volumeRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteVolumeUseCase(
            IVolumeRepository volumeRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork)
        {
            _volumeRepository = volumeRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task ExecuteAsync(int volumeId, CancellationToken ct)
        {
            if (volumeId <= 0)
            {
                throw new BadRequestException("Invalid volume id");
            }

            var volume = await _volumeRepository.GetByIdForDeleteAsync(volumeId, ct);
            if (volume == null)
            {
                throw new NotFoundException("Volume not found");
            }

            if (!VolumePermissionHelper.CanManage(_currentUserService, volume.Novel.AuthorId))
            {
                throw new ForbiddenException("You do not have permission to edit this novel");
            }

            volume.Novel.TotalVolumes = Math.Max(0, volume.Novel.TotalVolumes - 1);
            volume.Novel.TotalChapters = Math.Max(0, volume.Novel.TotalChapters - volume.Chapters.Count);
            _volumeRepository.Delete(volume);
            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
