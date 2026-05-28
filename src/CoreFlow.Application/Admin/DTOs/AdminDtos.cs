using CoreFlow.Domain.Enums;

namespace CoreFlow.Application.Admin.DTOs;

public class AdminCompanySummaryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Document { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int UsersCount { get; set; }
    public int VehiclesCount { get; set; }
    public int LeadsCount { get; set; }
    public int AvailableVehicles { get; set; }
    public int ReservedVehicles { get; set; }
    public int SoldVehicles { get; set; }
    public int NewLeads { get; set; }
    public int NegotiatingLeads { get; set; }
    public int ConvertedLeads { get; set; }
    public int LostLeads { get; set; }
}

public class AdminUserResponse
{
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class AdminOverviewResponse
{
    public List<AdminCompanySummaryResponse> Companies { get; set; } = new();
    public List<AdminUserResponse> Users { get; set; } = new();
}

public class CreateStoreUserRequest
{
    public Guid CompanyId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public UserRole Role { get; set; }
}

public class UpdateStoreUserRequest
{
    public Guid CompanyId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public UserRole Role { get; set; }
}

public class ResetUserPasswordRequest
{
    public string NewPassword { get; set; } = string.Empty;
}
