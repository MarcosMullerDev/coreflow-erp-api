using CoreFlow.Application.Vehicles.Repositories;
using CoreFlow.Domain.Entities;
using CoreFlow.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CoreFlow.Infrastructure.Repositories;

public class VehicleRepository : IVehicleRepository
{
    private readonly AppDbContext _context;

    public VehicleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Vehicle vehicle)
    {
        await _context.Vehicles.AddAsync(vehicle);
    }

    public async Task<List<Vehicle>> GetAllByCompanyAsync(Guid companyId)
    {
        return await _context.Vehicles
            .AsNoTracking()
            .Where(v => v.CompanyId == companyId && !v.IsDeleted)
            .OrderByDescending(v => v.CreatedAt)
            .ToListAsync();
    }

    public async Task<Vehicle?> GetByIdAndCompanyAsync(Guid id, Guid companyId)
    {
        return await _context.Vehicles
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.Id == id && v.CompanyId == companyId && !v.IsDeleted);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
    public async Task<Vehicle?> GetByIdAndCompanyForUpdateAsync(Guid id, Guid companyId)
    {
        return await _context.Vehicles
            .FirstOrDefaultAsync(v => v.Id == id && v.CompanyId == companyId && !v.IsDeleted);
    }
}