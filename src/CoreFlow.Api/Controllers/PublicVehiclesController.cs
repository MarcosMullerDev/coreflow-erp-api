using CoreFlow.Application.Common;
using CoreFlow.Infrastructure.Persistence;
using CoreFlow.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreFlow.Api.Controllers;

[ApiController]
[Route("api/Public/Vehicles")]
public class PublicVehiclesController : ControllerBase
{
    private readonly AppDbContext _context;

    public PublicVehiclesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAvailableVehicles()
    {
        var vehicles = await _context.Vehicles
            .AsNoTracking()
            .Include(v => v.Images)
            .Include(v => v.Optionals)
            .Where(v => !v.IsDeleted && v.Status == VehicleStatus.Available)
            .OrderByDescending(v => v.CreatedAt)
            .Select(v => new
            {
                v.Id,
                v.Brand,
                v.Model,
                v.Year,
                v.ModelYear,
                v.Mileage,
                v.Color,
                v.Plate,
                v.FuelType,
                v.TransmissionType,
                v.SalePrice,
                v.IsFeatured,
                v.VehicleType,
                v.Category,
                v.IsSingleOwner,
                v.IsBelowFipe,

                Optionals = v.Optionals
                    .Where(o => !o.IsDeleted)
                    .Select(o => o.Name),

                Images = v.Images.Select(i => new
                {
                    i.Id,
                    i.FileName,
                    i.IsPrimary,
                    Url = $"{Request.Scheme}://{Request.Host}/uploads/vehicles/{i.FileName}"
                }),

                PrimaryImage = v.Images
                    .Where(i => i.IsPrimary)
                    .Select(i => $"{Request.Scheme}://{Request.Host}/uploads/vehicles/{i.FileName}")
                    .FirstOrDefault()
            })
            .ToListAsync();

        return Ok(ApiResponse<object>.Ok(
            vehicles,
            "Available vehicles retrieved successfully."
        ));
    }

    [HttpGet("featured")]
    public async Task<IActionResult> GetFeaturedVehicles()
    {
        var vehicles = await _context.Vehicles
            .AsNoTracking()
            .Include(v => v.Images)
            .Include(v => v.Optionals)
            .Where(v =>
                !v.IsDeleted &&
                v.Status == VehicleStatus.Available &&
                v.IsFeatured)
            .OrderByDescending(v => v.CreatedAt)
            .Take(6)
            .Select(v => new
            {
                v.Id,
                v.Brand,
                v.Model,
                v.Year,
                v.ModelYear,
                v.Mileage,
                v.Color,
                v.Plate,
                v.FuelType,
                v.TransmissionType,
                v.SalePrice,
                v.IsFeatured,
                v.VehicleType,
                v.Category,
                v.IsSingleOwner,
                v.IsBelowFipe,

                Optionals = v.Optionals
                    .Where(o => !o.IsDeleted)
                    .Select(o => o.Name),

                Images = v.Images.Select(i => new
                {
                    i.Id,
                    i.FileName,
                    i.IsPrimary,
                    Url = $"{Request.Scheme}://{Request.Host}/uploads/vehicles/{i.FileName}"
                }),

                PrimaryImage = v.Images
                    .Where(i => i.IsPrimary)
                    .Select(i => $"{Request.Scheme}://{Request.Host}/uploads/vehicles/{i.FileName}")
                    .FirstOrDefault()
            })
            .ToListAsync();

        return Ok(ApiResponse<object>.Ok(
            vehicles,
            "Featured vehicles retrieved successfully."
        ));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetVehicleById(Guid id)
    {
        var vehicle = await _context.Vehicles
            .AsNoTracking()
            .Include(v => v.Images)
            .Include(v => v.Optionals)
            .FirstOrDefaultAsync(v =>
                v.Id == id &&
                !v.IsDeleted &&
                v.Status == VehicleStatus.Available);

        if (vehicle is null)
            return NotFound(ApiResponse<object>.Fail("Vehicle not found."));

        return Ok(ApiResponse<object>.Ok(new
        {
            vehicle.Id,
            vehicle.Brand,
            vehicle.Model,
            vehicle.Year,
            vehicle.ModelYear,
            vehicle.Mileage,
            vehicle.Color,
            vehicle.Plate,
            vehicle.Chassis,
            vehicle.FuelType,
            vehicle.TransmissionType,
            vehicle.SalePrice,
            vehicle.IsFeatured,
            vehicle.VehicleType,
            vehicle.Category,
            vehicle.IsSingleOwner,
            vehicle.IsBelowFipe,

            Optionals = vehicle.Optionals
                .Where(o => !o.IsDeleted)
                .Select(o => o.Name),

            Images = vehicle.Images.Select(i => new
            {
                i.Id,
                i.FileName,
                i.IsPrimary,
                Url = $"{Request.Scheme}://{Request.Host}/uploads/vehicles/{i.FileName}"
            }),

            PrimaryImage = vehicle.Images
                .Where(i => i.IsPrimary)
                .Select(i => $"{Request.Scheme}://{Request.Host}/uploads/vehicles/{i.FileName}")
                .FirstOrDefault()

        }, "Vehicle retrieved successfully."));
    }
}