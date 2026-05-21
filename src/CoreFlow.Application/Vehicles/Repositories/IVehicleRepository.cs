using CoreFlow.Domain.Entities;

namespace CoreFlow.Application.Vehicles.Repositories;

public interface IVehicleRepository
{
    Task AddAsync(Vehicle vehicle);
    Task<List<Vehicle>> GetAllByCompanyAsync(Guid companyId);
    Task<Vehicle?> GetByIdAndCompanyAsync(Guid id, Guid companyId);
    Task<Vehicle?> GetByIdAndCompanyForUpdateAsync(Guid id, Guid companyId);
    Task SaveChangesAsync();
}