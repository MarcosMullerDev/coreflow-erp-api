using CoreFlow.Application.Auth.DTOs;
using CoreFlow.Application.Auth.Services;
using CoreFlow.Application.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CoreFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [Authorize(Roles = "SystemAdmin,Admin")]
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserRequest request)
    {
        var response = await _authService.RegisterAsync(request);
        return Ok(ApiResponse<object>.Ok(response, "User registered successfully."));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var response = await _authService.LoginAsync(request);
        return Ok(ApiResponse<object>.Ok(response, "Login successful."));
    }
}