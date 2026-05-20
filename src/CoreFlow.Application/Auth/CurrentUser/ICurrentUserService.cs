namespace CoreFlow.Application.Auth.CurrentUser;

public interface ICurrentUserService
{
    Guid UserId { get; }
    Guid CompanyId { get; }
    string Role { get; }
    bool IsAuthenticated { get; }
}