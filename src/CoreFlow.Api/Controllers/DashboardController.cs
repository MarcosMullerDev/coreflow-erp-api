using CoreFlow.Application.Common;
using CoreFlow.Application.Dashboard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreFlow.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> Summary()
    {
        var result = await _dashboardService.GetSummaryAsync();

        return Ok(ApiResponse<object>.Ok(result));
    }

    [HttpGet("motors-summary")]
    public async Task<IActionResult> MotorsSummary()
    {
        var result = await _dashboardService.GetMotorsSummaryAsync();

        return Ok(ApiResponse<object>.Ok(result, "Motors dashboard retrieved successfully."));
    }
}