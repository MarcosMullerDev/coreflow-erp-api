using CoreFlow.Application.Auth.CurrentUser;
using CoreFlow.Application.Vehicles.DTOs;
using CoreFlow.Application.Vehicles.Repositories;
using CoreFlow.Domain.Entities;

namespace CoreFlow.Application.Vehicles.Services;

public class VehicleService : IVehicleService
{
    private readonly IVehicleRepository _repository;
    private readonly ICurrentUserService _currentUser;

    public VehicleService(
        IVehicleRepository repository,
        ICurrentUserService currentUser)
    {
        _repository = repository;
        _currentUser = currentUser;
    }

    public async Task<Vehicle> CreateAsync(CreateVehicleRequest request)
    {
        var vehicle = new Vehicle(
            _currentUser.CompanyId,
            request.Brand,
            request.Model,
            request.Year,
            request.ModelYear,
            request.Mileage,
            request.Color,
            request.Plate,
            request.Chassis,
            request.FuelType,
            request.TransmissionType,
            request.PurchasePrice,
            request.SalePrice
        );

        await _repository.AddAsync(vehicle);
        await _repository.SaveChangesAsync();

        return vehicle;
    }

    public async Task<List<Vehicle>> GetAllAsync()
    {
        return await _repository.GetAllByCompanyAsync(_currentUser.CompanyId);
    }

    public async Task<Vehicle?> GetByIdAsync(Guid id)
    {
        return await _repository.GetByIdAndCompanyAsync(id, _currentUser.CompanyId);
    }
    public async Task<Vehicle?> ToggleFeaturedAsync(Guid id)
    {
        var vehicle = await _repository.GetByIdAndCompanyForUpdateAsync(
            id,
            _currentUser.CompanyId
        );

        if (vehicle is null)
            return null;

        vehicle.SetFeatured(!vehicle.IsFeatured);

        await _repository.SaveChangesAsync();

        return vehicle;
    }
}