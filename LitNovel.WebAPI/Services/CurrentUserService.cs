using System.Security.Claims;
using LitNovel.Application.Common.Exceptions;
using LitNovel.Application.Common.Interfaces.Services;

namespace LitNovel.WebAPI.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated == true;

        public string? Role => _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role);

        public string? IpAddress
        {
            get
            {
                var context = _httpContextAccessor.HttpContext;
                var forwardedFor = context?.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                return forwardedFor?.Split(',').FirstOrDefault()?.Trim()
                    ?? context?.Connection.RemoteIpAddress?.ToString();
            }
        }

        public int UserId
        {
            get
            {
                var value = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
                return int.TryParse(value, out var userId) ? userId : throw new UnauthorizedException("User is not authenticated");
            }
        }
    }
}
