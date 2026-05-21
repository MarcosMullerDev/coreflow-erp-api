using CoreFlow.Application.Dashboard.DTOs;

namespace CoreFlow.Application.Dashboard.Services;

public interface IDashboardService
{
    Task<DashboardSummaryResponse> GetSummaryAsync();
    Task<MotorsDashboardSummaryResponse> GetMotorsSummaryAsync();
}