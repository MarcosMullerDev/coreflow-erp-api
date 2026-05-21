namespace CoreFlow.Application.Dashboard.DTOs;

public class MotorsDashboardSummaryResponse
{
    public int TotalVehicles { get; set; }
    public int AvailableVehicles { get; set; }
    public int ReservedVehicles { get; set; }
    public int SoldVehicles { get; set; }
    public decimal TotalInventoryValue { get; set; }
    public decimal PotentialRevenue { get; set; }
}