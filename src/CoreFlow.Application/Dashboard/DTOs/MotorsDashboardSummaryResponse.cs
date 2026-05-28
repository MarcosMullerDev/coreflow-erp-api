namespace CoreFlow.Application.Dashboard.DTOs;

public class MotorsDashboardSummaryResponse
{
    public int TotalVehicles { get; set; }
    public int AvailableVehicles { get; set; }
    public int ReservedVehicles { get; set; }
    public int SoldVehicles { get; set; }
    public decimal TotalInventoryValue { get; set; }
    public decimal PotentialRevenue { get; set; }
    public int TotalLeads { get; set; }
    public int NewLeads { get; set; }
    public int NegotiatingLeads { get; set; }
    public int ConvertedLeads { get; set; }
    public int LostLeads { get; set; }

    public int OverdueNewLeads { get; set; }

    public decimal ConversionRate { get; set; }
}