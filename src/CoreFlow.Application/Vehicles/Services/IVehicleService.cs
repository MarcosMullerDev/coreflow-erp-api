using CoreFlow.Application.Vehicles.DTOs;
using CoreFlow.Domain.Entities;

namespace CoreFlow.Application.Vehicles.Services;

public interface IVehicleService
{
    Task<Vehicle> CreateAsync(CreateVehicleRequest request);
    Task<List<Vehicle>> GetAllAsync();
    Task<Vehicle?> GetByIdAsync(Guid id);
    Task<Vehicle?> ToggleFeaturedAsync(Guid id);
}