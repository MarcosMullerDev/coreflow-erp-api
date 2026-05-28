using CoreFlow.Application.Auth.CurrentUser;
using CoreFlow.Application.Common;
using CoreFlow.Application.Leads.DTOs;
using CoreFlow.Domain.Entities;
using CoreFlow.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreFlow.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/Leads/{leadId:guid}/Notes")]
public class LeadNotesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public LeadNotesController(AppDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(Guid leadId)
    {
        var leadExists = await _context.Leads
            .AnyAsync(l =>
                l.Id == leadId &&
                l.CompanyId == _currentUser.CompanyId &&
                !l.IsDeleted);

        if (!leadExists)
            return NotFound(ApiResponse<object>.Fail("Lead not found."));

        var notes = await _context.LeadNotes
            .AsNoTracking()
            .Include(n => n.User)
            .Where(n =>
                n.LeadId == leadId &&
                n.CompanyId == _currentUser.CompanyId &&
                !n.IsDeleted)
            .OrderByDescending(n => n.CreatedAt)
            .Select(n => new
            {
                n.Id,
                n.Note,
                n.FollowUpAt,
                n.CreatedAt,
                User = n.User == null ? null : new
                {
                    n.User.Id,
                    n.User.Name,
                    n.User.Email
                }
            })
            .ToListAsync();

        return Ok(ApiResponse<object>.Ok(notes, "Lead notes retrieved successfully."));
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager,Sales")]
    public async Task<IActionResult> Create(Guid leadId, CreateLeadNoteRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Note))
            return BadRequest(ApiResponse<object>.Fail("Note is required."));

        var lead = await _context.Leads
            .FirstOrDefaultAsync(l =>
                l.Id == leadId &&
                l.CompanyId == _currentUser.CompanyId &&
                !l.IsDeleted);

        if (lead is null)
            return NotFound(ApiResponse<object>.Fail("Lead not found."));

        var note = new LeadNote(
            _currentUser.CompanyId,
            lead.Id,
            _currentUser.UserId,
            request.Note.Trim(),
            request.FollowUpAt
        );

        await _context.LeadNotes.AddAsync(note);
        await _context.SaveChangesAsync();

        return Ok(ApiResponse<object>.Ok(note, "Lead note created successfully."));
    }

    [HttpDelete("{noteId:guid}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Delete(Guid leadId, Guid noteId)
    {
        var note = await _context.LeadNotes
            .FirstOrDefaultAsync(n =>
                n.Id == noteId &&
                n.LeadId == leadId &&
                n.CompanyId == _currentUser.CompanyId &&
                !n.IsDeleted);

        if (note is null)
            return NotFound(ApiResponse<object>.Fail("Lead note not found."));

        note.MarkAsDeleted();

        await _context.SaveChangesAsync();

        return Ok(ApiResponse<object>.Ok(new { }, "Lead note deleted successfully."));
    }
}
