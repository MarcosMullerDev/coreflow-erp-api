using CoreFlow.Application.Common;
using CoreFlow.Application.Vehicles.DTOs;
using CoreFlow.Application.Vehicles.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        return Ok(ApiResponse<object>.Ok(vehicles, "Vehicles retrieved successfully."));
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
}