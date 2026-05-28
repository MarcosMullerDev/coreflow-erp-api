using CoreFlow.Application.Common;
using CoreFlow.Application.Vehicles.DTOs;
using CoreFlow.Application.Vehicles.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CoreFlow.Domain.Enums;

namespace CoreFlow.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class VehiclesController : ControllerBase
{
    private readonly IVehicleService _vehicleService;

    public VehiclesController(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager,Stock")]
    public async Task<IActionResult> Create(CreateVehicleRequest request)
    {
        var vehicle = await _vehicleService.CreateAsync(request);

        return Ok(ApiResponse<object>.Ok(vehicle, "Vehicle created successfully."));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var vehicles = await _vehicleService.GetAllAsync();

        var result = vehicles.Select(v => new
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
            v.PurchasePrice,
            v.SalePrice,
            v.Status,
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
        });

        return Ok(ApiResponse<object>.Ok(result, "Vehicles retrieved successfully."));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var vehicle = await _vehicleService.GetByIdAsync(id);

        if (vehicle is null)
            return NotFound(ApiResponse<object>.Fail("Vehicle not found."));

        return Ok(ApiResponse<object>.Ok(vehicle, "Vehicle retrieved successfully."));
    }
    [HttpPatch("{id:guid}/featured")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> ToggleFeatured(Guid id)
    {
        var vehicle = await _vehicleService.ToggleFeaturedAsync(id);

        if (vehicle is null)
            return NotFound(ApiResponse<object>.Fail("Vehicle not found."));

        return Ok(ApiResponse<object>.Ok(
            vehicle,
            vehicle.IsFeatured
                ? "Vehicle featured successfully."
                : "Vehicle removed from featured."
        ));
    }
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,Manager,Stock")]
    public async Task<IActionResult> Update(Guid id, UpdateVehicleRequest request)
    {
        var vehicle = await _vehicleService.UpdateAsync(id, request);

        if (vehicle is null)
            return NotFound(ApiResponse<object>.Fail("Vehicle not found."));

        return Ok(ApiResponse<object>.Ok(vehicle, "Vehicle updated successfully."));
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _vehicleService.DeleteAsync(id);

        if (!deleted)
            return NotFound(ApiResponse<object>.Fail("Vehicle not found."));

        return Ok(ApiResponse<object>.Ok(new { }, "Vehicle deleted successfully."));
    }

    [HttpPatch("{id:guid}/status")]
    [Authorize(Roles = "Admin,Manager,Stock")]
    public async Task<IActionResult> ChangeStatus(Guid id, [FromQuery] VehicleStatus status)
    {
        var vehicle = await _vehicleService.ChangeStatusAsync(id, status);

        if (vehicle is null)
            return NotFound(ApiResponse<object>.Fail("Vehicle not found."));

        return Ok(ApiResponse<object>.Ok(vehicle, "Vehicle status updated successfully."));
    }
}