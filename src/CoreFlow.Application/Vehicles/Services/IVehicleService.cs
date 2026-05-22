using CoreFlow.Application.Vehicles.DTOs;
using CoreFlow.Domain.Entities;
using CoreFlow.Domain.Enums;

namespace CoreFlow.Application.Vehicles.Services;

public interface IVehicleService
{
    Task<Vehicle> CreateAsync(CreateVehicleRequest request);
    Task<List<Vehicle>> GetAllAsync();
    Task<Vehicle?> GetByIdAsync(Guid id);
    Task<Vehicle?> ToggleFeaturedAsync(Guid id);
    Task<Vehicle?> UpdateAsync(Guid id, UpdateVehicleRequest request);
    Task<bool> DeleteAsync(Guid id);
    Task<Vehicle?> ChangeStatusAsync(Guid id, VehicleStatus status);
}