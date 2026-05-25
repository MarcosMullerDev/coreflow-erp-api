using CoreFlow.Application.Common;
using CoreFlow.Domain.Enums;
using CoreFlow.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreFlow.Api.Controllers;

[ApiController]
[Route("api/Public/Store")]
public class PublicStoreController : ControllerBase
{
    private readonly AppDbContext _context;

    public PublicStoreController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> GetStore(string slug)
    {
        var store = await _context.CompanySettings
            .AsNoTracking()
            .Include(s => s.Company)
            .FirstOrDefaultAsync(s =>
                s.Slug == slug &&
                !s.IsDeleted);

        if (store is null)
            return NotFound(ApiResponse<object>.Fail("Store not found."));

        return Ok(ApiResponse<object>.Ok(new
        {
            store.Id,
            store.CompanyId,
            store.StoreName,
            store.Slug,
            store.LogoUrl,
            store.BannerUrl,
            store.PrimaryColor,
            store.SecondaryColor,
            store.Whatsapp,
            store.Instagram,
            store.Address,
            store.HeroTitle,
            store.HeroSubtitle
        }, "Store retrieved successfully."));
    }

    [HttpGet("{slug}/vehicles")]
    public async Task<IActionResult> GetStoreVehicles(string slug)
    {
        var store = await _context.CompanySettings
            .AsNoTracking()
            .FirstOrDefaultAsync(s =>
                s.Slug == slug &&
                !s.IsDeleted);

        if (store is null)
            return NotFound(ApiResponse<object>.Fail("Store not found."));

        var vehicles = await _context.Vehicles
            .AsNoTracking()
            .Include(v => v.Images)
            .Include(v => v.Optionals)
            .Where(v =>
                v.CompanyId == store.CompanyId &&
                !v.IsDeleted &&
                v.Status == VehicleStatus.Available)
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

                Images = v.Images
                    .Where(i => !i.IsDeleted)
                    .Select(i => new
                    {
                        i.Id,
                        i.FileName,
                        i.IsPrimary,
                        Url = $"{Request.Scheme}://{Request.Host}/uploads/vehicles/{i.FileName}"
                    }),

                PrimaryImage = v.Images
                    .Where(i => i.IsPrimary && !i.IsDeleted)
                    .Select(i => $"{Request.Scheme}://{Request.Host}/uploads/vehicles/{i.FileName}")
                    .FirstOrDefault()
            })
            .ToListAsync();

        return Ok(ApiResponse<object>.Ok(
            vehicles,
            "Store vehicles retrieved successfully."
        ));
    }

    [HttpGet("{slug}/vehicles/featured")]
    public async Task<IActionResult> GetStoreFeaturedVehicles(string slug)
    {
        var store = await _context.CompanySettings
            .AsNoTracking()
            .FirstOrDefaultAsync(s =>
                s.Slug == slug &&
                !s.IsDeleted);

        if (store is null)
            return NotFound(ApiResponse<object>.Fail("Store not found."));

        var vehicles = await _context.Vehicles
            .AsNoTracking()
            .Include(v => v.Images)
            .Where(v =>
                v.CompanyId == store.CompanyId &&
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
                v.FuelType,
                v.TransmissionType,
                v.SalePrice,
                v.IsFeatured,

                PrimaryImage = v.Images
                    .Where(i => i.IsPrimary && !i.IsDeleted)
                    .Select(i => $"{Request.Scheme}://{Request.Host}/uploads/vehicles/{i.FileName}")
                    .FirstOrDefault()
            })
            .ToListAsync();

        return Ok(ApiResponse<object>.Ok(
            vehicles,
            "Store featured vehicles retrieved successfully."
        ));
    }
}