namespace LitNovel.Application.Common.Interfaces.Services
{
    public interface ICurrentUserService
    {
        int UserId { get; }
        string? Role { get; }
        bool IsAuthenticated { get; }
    }
}
