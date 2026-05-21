using CoreFlow.Application.Auth.CurrentUser;
using CoreFlow.Application.Common;
using CoreFlow.Domain.Enums;
using CoreFlow.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreFlow.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class LeadsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public LeadsController(AppDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var leads = await _context.Leads
            .AsNoTracking()
            .Include(l => l.Vehicle)
            .Where(l => l.CompanyId == _currentUser.CompanyId && !l.IsDeleted)
            .OrderByDescending(l => l.CreatedAt)
            .Select(l => new
            {
                l.Id,
                l.Name,
                l.Phone,
                l.Email,
                l.Message,
                l.Status,
                l.CreatedAt,
                Vehicle = new
                {
                    l.Vehicle.Id,
                    l.Vehicle.Brand,
                    l.Vehicle.Model,
                    l.Vehicle.Year,
                    l.Vehicle.ModelYear,
                    l.Vehicle.SalePrice
                }
            })
            .ToListAsync();

        return Ok(ApiResponse<object>.Ok(leads, "Leads retrieved successfully."));
    }

    [HttpPatch("{id:guid}/status")]
    [Authorize(Roles = "Admin,Manager,Sales")]
    public async Task<IActionResult> UpdateStatus(Guid id, LeadStatus status)
    {
        var lead = await _context.Leads
            .FirstOrDefaultAsync(l =>
                l.Id == id &&
                l.CompanyId == _currentUser.CompanyId &&
                !l.IsDeleted);

        if (lead is null)
            return NotFound(ApiResponse<object>.Fail("Lead not found."));

        lead.UpdateStatus(status);

        await _context.SaveChangesAsync();

        return Ok(ApiResponse<object>.Ok(lead, "Lead status updated successfully."));
    }
}