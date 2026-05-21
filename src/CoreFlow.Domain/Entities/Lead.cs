using CoreFlow.Domain.Common;
using CoreFlow.Domain.Enums;

namespace CoreFlow.Domain.Entities;

public class Lead : BaseEntity
{
    public Guid CompanyId { get; private set; }
    public Company Company { get; private set; } = null!;

    public Guid VehicleId { get; private set; }
    public Vehicle Vehicle { get; private set; } = null!;

    public string Name { get; private set; } = string.Empty;
    public string Phone { get; private set; } = string.Empty;
    public string? Email { get; private set; }
    public string? Message { get; private set; }

    public LeadStatus Status { get; private set; }

    private Lead() { }

    public Lead(
        Guid companyId,
        Guid vehicleId,
        string name,
        string phone,
        string? email,
        string? message)
    {
        CompanyId = companyId;
        VehicleId = vehicleId;
        Name = name;
        Phone = phone;
        Email = email;
        Message = message;
        Status = LeadStatus.New;
    }

    public void UpdateStatus(LeadStatus status)
    {
        Status = status;
        SetUpdated();
    }
}