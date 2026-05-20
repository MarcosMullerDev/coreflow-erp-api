using System.Security.Claims;
using CoreFlow.Application.Auth.CurrentUser;
using Microsoft.AspNetCore.Http;

namespace CoreFlow.Infrastructure.Auth;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public bool IsAuthenticated =>
        _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;

    public Guid UserId
    {
        get
        {
            var userId = _httpContextAccessor.HttpContext?.User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return Guid.TryParse(userId, out var id) ? id : Guid.Empty;
        }
    }

    public Guid CompanyId
    {
        get
        {
            var companyId = _httpContextAccessor.HttpContext?.User
                .FindFirst("companyId")?.Value;

            return Guid.TryParse(companyId, out var id) ? id : Guid.Empty;
        }
    }

    public string Role =>
        _httpContextAccessor.HttpContext?.User
            .FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
}