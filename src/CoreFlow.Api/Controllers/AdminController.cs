using CoreFlow.Application.Admin.DTOs;
using CoreFlow.Application.Admin.Services;
using CoreFlow.Application.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreFlow.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [Authorize(Roles = "SystemAdmin")]
    [HttpGet("system")]
    public async Task<IActionResult> GetSystemOverview()
    {
        var response = await _adminService.GetSystemOverviewAsync();
        return Ok(ApiResponse<object>.Ok(response, "System overview loaded."));
    }

    [Authorize(Roles = "SystemAdmin,Admin,Manager")]
    [HttpGet("store")]
    public async Task<IActionResult> GetStoreOverview()
    {
        var response = await _adminService.GetCurrentStoreOverviewAsync();
        return Ok(ApiResponse<object>.Ok(response, "Store overview loaded."));
    }

    [Authorize(Roles = "SystemAdmin,Admin")]
    [HttpPost("users")]
    public async Task<IActionResult> CreateUser(CreateStoreUserRequest request)
    {
        var response = await _adminService.CreateUserAsync(request);
        return Ok(ApiResponse<object>.Ok(response, "User created successfully."));
    }

    [Authorize(Roles = "SystemAdmin,Admin")]
    [HttpPut("users/{id:guid}")]
    public async Task<IActionResult> UpdateUser(Guid id, UpdateStoreUserRequest request)
    {
        var response = await _adminService.UpdateUserAsync(id, request);
        return Ok(ApiResponse<object>.Ok(response, "User updated successfully."));
    }

    [Authorize(Roles = "SystemAdmin,Admin")]
    [HttpPatch("users/{id:guid}/activate")]
    public async Task<IActionResult> ActivateUser(Guid id)
    {
        await _adminService.ActivateUserAsync(id);
        return Ok(ApiResponse<object>.Ok(null, "User activated successfully."));
    }

    [Authorize(Roles = "SystemAdmin,Admin")]
    [HttpPatch("users/{id:guid}/deactivate")]
    public async Task<IActionResult> DeactivateUser(Guid id)
    {
        await _adminService.DeactivateUserAsync(id);
        return Ok(ApiResponse<object>.Ok(null, "User deactivated successfully."));
    }

    [Authorize(Roles = "SystemAdmin,Admin")]
    [HttpPatch("users/{id:guid}/reset-password")]
    public async Task<IActionResult> ResetPassword(Guid id, ResetUserPasswordRequest request)
    {
        await _adminService.ResetPasswordAsync(id, request);
        return Ok(ApiResponse<object>.Ok(null, "Password reset successfully."));
    }

    [Authorize(Roles = "SystemAdmin")]
    [HttpPatch("companies/{id:guid}/activate")]
    public async Task<IActionResult> ActivateCompany(Guid id)
    {
        await _adminService.ActivateCompanyAsync(id);
        return Ok(ApiResponse<object>.Ok(null, "Company activated successfully."));
    }

    [Authorize(Roles = "SystemAdmin")]
    [HttpPatch("companies/{id:guid}/deactivate")]
    public async Task<IActionResult> DeactivateCompany(Guid id)
    {
        await _adminService.DeactivateCompanyAsync(id);
        return Ok(ApiResponse<object>.Ok(null, "Company deactivated successfully."));
    }
}
