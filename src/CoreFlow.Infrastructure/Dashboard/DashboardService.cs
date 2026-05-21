using CoreFlow.Application.Auth.CurrentUser;
using CoreFlow.Application.Dashboard.DTOs;
using CoreFlow.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using CoreFlow.Application.Dashboard.Services;
using CoreFlow.Domain.Enums;

namespace CoreFlow.Infrastructure.Dashboard;
public class DashboardService : IDashboardService
{
    private readonly AppDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public DashboardService(
        AppDbContext context,
        ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<DashboardSummaryResponse> GetSummaryAsync()
    {
        var companyId = _currentUser.CompanyId;

        var totalRevenue = await _context.Sales
            .Where(s => s.CompanyId == companyId && !s.IsDeleted)
            .SumAsync(s => (decimal?)s.TotalAmount) ?? 0;

        var totalSales = await _context.Sales
            .CountAsync(s => s.CompanyId == companyId && !s.IsDeleted);

        var totalProducts = await _context.Products
            .CountAsync(p => p.CompanyId == companyId && !p.IsDeleted);

        var lowStockProducts = await _context.Products
            .CountAsync(p =>
                p.CompanyId == companyId &&
                !p.IsDeleted &&
                p.StockQuantity <= 5);

        return new DashboardSummaryResponse
        {
            TotalRevenue = totalRevenue,
            TotalSales = totalSales,
            TotalProducts = totalProducts,
            LowStockProducts = lowStockProducts
        };
    }

    public async Task<MotorsDashboardSummaryResponse> GetMotorsSummaryAsync()
    {
        var companyId = _currentUser.CompanyId;

        var totalVehicles = await _context.Vehicles
            .CountAsync(v => v.CompanyId == companyId && !v.IsDeleted);

        var availableVehicles = await _context.Vehicles
            .CountAsync(v =>
                v.CompanyId == companyId &&
                !v.IsDeleted &&
                v.Status == VehicleStatus.Available);

        var reservedVehicles = await _context.Vehicles
            .CountAsync(v =>
                v.CompanyId == companyId &&
                !v.IsDeleted &&
                v.Status == VehicleStatus.Reserved);

        var soldVehicles = await _context.Vehicles
            .CountAsync(v =>
                v.CompanyId == companyId &&
                !v.IsDeleted &&
                v.Status == VehicleStatus.Sold);

        var totalInventoryValue = await _context.Vehicles
            .Where(v =>
                v.CompanyId == companyId &&
                !v.IsDeleted &&
                v.Status == VehicleStatus.Available)
            .SumAsync(v => (decimal?)v.PurchasePrice) ?? 0;

        var potentialRevenue = await _context.Vehicles
            .Where(v =>
                v.CompanyId == companyId &&
                !v.IsDeleted &&
                v.Status == VehicleStatus.Available)
            .SumAsync(v => (decimal?)v.SalePrice) ?? 0;

        return new MotorsDashboardSummaryResponse
        {
            TotalVehicles = totalVehicles,
            AvailableVehicles = availableVehicles,
            ReservedVehicles = reservedVehicles,
            SoldVehicles = soldVehicles,
            TotalInventoryValue = totalInventoryValue,
            PotentialRevenue = potentialRevenue
        };
    }
}