namespace CoreFlow.Application.Dashboard.DTOs;

public class DashboardSummaryResponse
{
    public decimal TotalRevenue { get; set; }
    public int TotalSales { get; set; }
    public int TotalProducts { get; set; }
    public int LowStockProducts { get; set; }
}