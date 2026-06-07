namespace LitNovel.Application.Common.Interfaces.Services
{
    public interface ICurrentUserService
    {
        int UserId { get; }
        bool IsAuthenticated { get; }
    }
}
