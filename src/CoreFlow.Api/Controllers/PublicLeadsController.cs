using CoreFlow.Application.Common;
using CoreFlow.Application.Leads.DTOs;
using CoreFlow.Domain.Entities;
using CoreFlow.Domain.Enums;
using CoreFlow.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreFlow.Api.Controllers;

[ApiController]
[Route("api/Public/Leads")]
public class PublicLeadsController : ControllerBase
{
    private readonly AppDbContext _context;

    public PublicLeadsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateLeadRequest request)
    {
        var vehicle = await _context.Vehicles
            .AsNoTracking()
            .FirstOrDefaultAsync(v =>
                v.Id == request.VehicleId &&
                !v.IsDeleted &&
                v.Status == VehicleStatus.Available);

        if (vehicle is null)
            return NotFound(ApiResponse<object>.Fail("Vehicle not found."));

        var lead = new Lead(
            vehicle.CompanyId,
            vehicle.Id,
            request.Name,
            request.Phone,
            request.Email,
            request.Message
        );

        await _context.Leads.AddAsync(lead);
        await _context.SaveChangesAsync();

        return Ok(ApiResponse<object>.Ok(lead, "Lead created successfully."));
    }
}