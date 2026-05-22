// src/CoreFlow.Domain/Entities/VehicleOptional.cs

using CoreFlow.Domain.Common;

namespace CoreFlow.Domain.Entities;

public class VehicleOptional : BaseEntity
{
    public Guid VehicleId { get; private set; }
    public Vehicle Vehicle { get; private set; } = null!;
    public string Name { get; private set; } = string.Empty;

    private VehicleOptional() { }

    public VehicleOptional(Guid vehicleId, string name)
    {
        VehicleId = vehicleId;
        Name = name;
    }
}