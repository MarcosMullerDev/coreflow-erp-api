using CoreFlow.Application.Admin.DTOs;
using CoreFlow.Application.Auth.CurrentUser;
using CoreFlow.Application.Security;
using CoreFlow.Domain.Entities;
using CoreFlow.Domain.Enums;
using CoreFlow.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using CoreFlow.Application.Admin.Services;

namespace CoreFlow.Infrastructure.Admin.Services;

public class AdminService : IAdminService
{
    private readonly AppDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public AdminService(AppDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<AdminOverviewResponse> GetSystemOverviewAsync()
    {
        EnsureSystemAdmin();

        return new AdminOverviewResponse
        {
            Companies = await BuildCompanies(_context.Companies).ToListAsync(),
            Users = await BuildUsers(_context.Users.Include(u => u.Company)).ToListAsync()
        };
    }

    public async Task<AdminOverviewResponse> GetCurrentStoreOverviewAsync()
    {
        EnsureAdminOrManager();
        var companyId = _currentUser.CompanyId;

        return new AdminOverviewResponse
        {
            Companies = await BuildCompanies(_context.Companies.Where(c => c.Id == companyId)).ToListAsync(),
            Users = await BuildUsers(_context.Users.Include(u => u.Company).Where(u => u.CompanyId == companyId)).ToListAsync()
        };
    }

    public async Task<AdminUserResponse> CreateUserAsync(CreateStoreUserRequest request)
    {
        EnsureCanManageCompany(request.CompanyId);

        if (request.Role == UserRole.SystemAdmin && !IsSystemAdmin())
            throw new Exception("Only SystemAdmin can create SystemAdmin users.");

        var companyExists = await _context.Companies.AnyAsync(c => c.Id == request.CompanyId && c.IsActive);
        if (!companyExists) throw new Exception("Company not found or inactive.");

        if (request.Password.Length < 6) throw new Exception("Password must have at least 6 characters.");

        var email = request.Email.Trim().ToLowerInvariant();
        if (await _context.Users.AnyAsync(u => u.Email == email)) throw new Exception("User already exists.");

        var user = new User(request.CompanyId, request.Name, email, PasswordHasher.Hash(request.Password), request.Role);
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return await GetUser(user.Id);
    }

    public async Task<AdminUserResponse> UpdateUserAsync(Guid userId, UpdateStoreUserRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId) ?? throw new Exception("User not found.");
        EnsureCanManageCompany(user.CompanyId);
        EnsureCanManageCompany(request.CompanyId);

        if ((user.Role == UserRole.SystemAdmin || request.Role == UserRole.SystemAdmin) && !IsSystemAdmin())
            throw new Exception("Only SystemAdmin can manage SystemAdmin users.");

        var email = request.Email.Trim().ToLowerInvariant();
        if (await _context.Users.AnyAsync(u => u.Id != userId && u.Email == email)) throw new Exception("Email already in use.");

        user.ChangeCompany(request.CompanyId);
        user.Update(request.Name, email, request.Role);
        await _context.SaveChangesAsync();
        return await GetUser(user.Id);
    }

    public async Task ActivateUserAsync(Guid userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId) ?? throw new Exception("User not found.");
        EnsureCanManageCompany(user.CompanyId);
        user.Activate();
        await _context.SaveChangesAsync();
    }

    public async Task DeactivateUserAsync(Guid userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId) ?? throw new Exception("User not found.");
        EnsureCanManageCompany(user.CompanyId);
        if (user.Id == _currentUser.UserId) throw new Exception("You cannot deactivate your own user.");
        user.Deactivate();
        await _context.SaveChangesAsync();
    }

    public async Task ResetPasswordAsync(Guid userId, ResetUserPasswordRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId) ?? throw new Exception("User not found.");
        EnsureCanManageCompany(user.CompanyId);
        if (request.NewPassword.Length < 6) throw new Exception("Password must have at least 6 characters.");
        user.ResetPassword(PasswordHasher.Hash(request.NewPassword));
        await _context.SaveChangesAsync();
    }

    public async Task ActivateCompanyAsync(Guid companyId)
    {
        EnsureSystemAdmin();
        var company = await _context.Companies.FirstOrDefaultAsync(c => c.Id == companyId) ?? throw new Exception("Company not found.");
        company.Activate();
        await _context.SaveChangesAsync();
    }

    public async Task DeactivateCompanyAsync(Guid companyId)
    {
        EnsureSystemAdmin();
        var company = await _context.Companies.FirstOrDefaultAsync(c => c.Id == companyId) ?? throw new Exception("Company not found.");
        company.Deactivate();
        await _context.SaveChangesAsync();
    }

    private IQueryable<AdminCompanySummaryResponse> BuildCompanies(IQueryable<Company> query)
    {
        return query.Select(c => new AdminCompanySummaryResponse
        {
            Id = c.Id,
            Name = c.Name,
            Document = c.Document,
            Email = c.Email,
            Phone = c.Phone,
            IsActive = c.IsActive,
            UsersCount = _context.Users.Count(u => u.CompanyId == c.Id),
            VehiclesCount = _context.Vehicles.Count(v => v.CompanyId == c.Id && !v.IsDeleted),
            LeadsCount = _context.Leads.Count(l => l.CompanyId == c.Id && !l.IsDeleted),
            AvailableVehicles = _context.Vehicles.Count(v => v.CompanyId == c.Id && !v.IsDeleted && (int)v.Status == 1),
            ReservedVehicles = _context.Vehicles.Count(v => v.CompanyId == c.Id && !v.IsDeleted && (int)v.Status == 2),
            SoldVehicles = _context.Vehicles.Count(v => v.CompanyId == c.Id && !v.IsDeleted && (int)v.Status == 3),
            NewLeads = _context.Leads.Count(l => l.CompanyId == c.Id && !l.IsDeleted && (int)l.Status == 1),
            NegotiatingLeads = _context.Leads.Count(l => l.CompanyId == c.Id && !l.IsDeleted && (int)l.Status == 3),
            ConvertedLeads = _context.Leads.Count(l => l.CompanyId == c.Id && !l.IsDeleted && (int)l.Status == 4),
            LostLeads = _context.Leads.Count(l => l.CompanyId == c.Id && !l.IsDeleted && (int)l.Status == 5)
        });
    }

    private IQueryable<AdminUserResponse> BuildUsers(IQueryable<User> query)
    {
        return query.Select(u => new AdminUserResponse
        {
            Id = u.Id,
            CompanyId = u.CompanyId,
            CompanyName = u.Company.Name,
            Name = u.Name,
            Email = u.Email,
            Role = u.Role.ToString(),
            IsActive = u.IsActive,
            CreatedAt = u.CreatedAt
        });
    }

    private async Task<AdminUserResponse> GetUser(Guid id)
    {
        return await BuildUsers(_context.Users.Include(u => u.Company).Where(u => u.Id == id)).FirstAsync();
    }

    private bool IsSystemAdmin() => _currentUser.Role == UserRole.SystemAdmin.ToString();

    private void EnsureSystemAdmin()
    {
        if (!IsSystemAdmin()) throw new Exception("Only SystemAdmin can access this resource.");
    }

    private void EnsureAdminOrManager()
    {
        if (IsSystemAdmin()) return;
        if (_currentUser.Role != UserRole.Admin.ToString() && _currentUser.Role != UserRole.Manager.ToString())
            throw new Exception("Only Admin or Manager can access this resource.");
    }

    private void EnsureCanManageCompany(Guid companyId)
    {
        if (IsSystemAdmin()) return;
        if (_currentUser.Role != UserRole.Admin.ToString()) throw new Exception("Only Admin can manage users.");
        if (_currentUser.CompanyId != companyId) throw new Exception("You cannot manage another company.");
    }
}
