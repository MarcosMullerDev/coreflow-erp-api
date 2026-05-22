using CoreFlow.Domain.Enums;

namespace CoreFlow.Application.Vehicles.DTOs;

public class UpdateVehicleRequest
{
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public int ModelYear { get; set; }
    public int Mileage { get; set; }
    public string Color { get; set; } = string.Empty;
    public string Plate { get; set; } = string.Empty;
    public string Chassis { get; set; } = string.Empty;
    public FuelType FuelType { get; set; }
    public TransmissionType TransmissionType { get; set; }
    public decimal PurchasePrice { get; set; }
    public decimal SalePrice { get; set; }
    public VehicleType VehicleType { get; set; }
    public VehicleCategory Category { get; set; }
    public bool IsSingleOwner { get; set; }
    public bool IsBelowFipe { get; set; }
    public List<string> Optionals { get; set; } = new();
}