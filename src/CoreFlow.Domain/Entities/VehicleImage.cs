using CoreFlow.Domain.Common;

namespace CoreFlow.Domain.Entities;

public class VehicleImage : BaseEntity
{
    public Guid VehicleId { get; private set; }
    public Vehicle Vehicle { get; private set; } = null!;

    public string FileName { get; private set; } = string.Empty;

    public bool IsPrimary { get; private set; }

    private VehicleImage() { }

    public VehicleImage(Guid vehicleId, string fileName, bool isPrimary)
    {
        VehicleId = vehicleId;
        FileName = fileName;
        IsPrimary = isPrimary;
    }
    public void SetPrimary(bool isPrimary)
    {
        IsPrimary = isPrimary;
        SetUpdated();
    }
}