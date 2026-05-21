namespace CoreFlow.Application.Leads.DTOs;

public class CreateLeadRequest
{
    public Guid VehicleId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Message { get; set; }
}