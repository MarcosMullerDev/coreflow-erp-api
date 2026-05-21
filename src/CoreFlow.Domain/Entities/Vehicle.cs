using CoreFlow.Domain.Common;
using CoreFlow.Domain.Enums;

namespace CoreFlow.Domain.Entities;

public class Vehicle : BaseEntity
{
    public Guid CompanyId { get; private set; }
    public Company Company { get; private set; } = null!;
    public string Brand { get; private set; } = string.Empty;
    public string Model { get; private set; } = string.Empty;
    public int Year { get; private set; }
    public int ModelYear { get; private set; }
    public int Mileage { get; private set; }
    public string Color { get; private set; } = string.Empty;
    public string Plate { get; private set; } = string.Empty;
    public string Chassis { get; private set; } = string.Empty;
    public FuelType FuelType { get; private set; }
    public TransmissionType TransmissionType { get; private set; }
    public decimal PurchasePrice { get; private set; }
    public decimal SalePrice { get; private set; }
    public VehicleStatus Status { get; private set; }
    public ICollection<VehicleImage> Images { get; private set; } = new List<VehicleImage>();
    public ICollection<Lead> Leads { get; private set; } = new List<Lead>();
    public bool IsFeatured { get; private set; }
    private Vehicle() { }

    public Vehicle(
        Guid companyId,
        string brand,
        string model,
        int year,
        int modelYear,
        int mileage,
        string color,
        string plate,
        string chassis,
        FuelType fuelType,
        TransmissionType transmissionType,
        decimal purchasePrice,
        decimal salePrice)
    {
        CompanyId = companyId;
        Brand = brand;
        Model = model;
        Year = year;
        ModelYear = modelYear;
        Mileage = mileage;
        Color = color;
        Plate = plate;
        Chassis = chassis;
        FuelType = fuelType;
        TransmissionType = transmissionType;
        PurchasePrice = purchasePrice;
        SalePrice = salePrice;
        Status = VehicleStatus.Available;
        IsFeatured = false;
    }

    public void MarkAsSold()
    {
        Status = VehicleStatus.Sold;
        SetUpdated();
    }

    public void Reserve()
    {
        Status = VehicleStatus.Reserved;
        SetUpdated();
    }

    public void SendToMaintenance()
    {
        Status = VehicleStatus.Maintenance;
        SetUpdated();
    }

    public void SetFeatured(bool featured)
    {
        IsFeatured = featured;
        SetUpdated();
    }
}