namespace LitNovel.Application.Common.Interfaces.Services
{
    public interface ICurrentUserService
    {
        int UserId { get; }
        string? Role { get; }
        string? IpAddress { get; }
        bool IsAuthenticated { get; }
    }
}
