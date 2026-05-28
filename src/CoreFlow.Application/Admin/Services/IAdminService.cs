using CoreFlow.Application.Admin.DTOs;

namespace CoreFlow.Application.Admin.Services;

public interface IAdminService
{
    Task<AdminOverviewResponse> GetSystemOverviewAsync();
    Task<AdminOverviewResponse> GetCurrentStoreOverviewAsync();
    Task<AdminUserResponse> CreateUserAsync(CreateStoreUserRequest request);
    Task<AdminUserResponse> UpdateUserAsync(Guid userId, UpdateStoreUserRequest request);
    Task ActivateUserAsync(Guid userId);
    Task DeactivateUserAsync(Guid userId);
    Task ResetPasswordAsync(Guid userId, ResetUserPasswordRequest request);
    Task ActivateCompanyAsync(Guid companyId);
    Task DeactivateCompanyAsync(Guid companyId);
}
