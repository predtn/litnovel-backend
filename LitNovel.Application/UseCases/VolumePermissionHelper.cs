using LitNovel.Application.Common.Interfaces.Services;
using LitNovel.Domain.Enums;

namespace LitNovel.Application.UseCases
{
    internal static class VolumePermissionHelper
    {
        public static bool CanManage(ICurrentUserService currentUserService, int authorId)
        {
            return authorId == currentUserService.UserId
                || string.Equals(currentUserService.Role, UserRole.Staff.ToString(), StringComparison.OrdinalIgnoreCase)
                || string.Equals(currentUserService.Role, UserRole.Admin.ToString(), StringComparison.OrdinalIgnoreCase);
        }
    }
}
