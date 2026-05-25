using CoreFlow.Application.Auth.CurrentUser;
using CoreFlow.Application.CompanySettings.DTOs;
using CoreFlow.Application.CompanySettings.Services;
using CoreFlow.Domain.Entities;
using CoreFlow.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CoreFlow.Infrastructure.Services;

public class CompanySettingsService : ICompanySettingsService
{
    private readonly AppDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public CompanySettingsService(
        AppDbContext context,
        ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<CompanySettings> GetAsync()
    {
        var settings = await _context.CompanySettings
            .FirstOrDefaultAsync(s =>
                s.CompanyId == _currentUser.CompanyId &&
                !s.IsDeleted);

        if (settings is not null)
            return settings;

        var company = await _context.Companies
            .FirstAsync(c => c.Id == _currentUser.CompanyId);

        settings = new CompanySettings(
            company.Id,
            company.Name,
            GenerateSlug(company.Name)
        );

        await _context.CompanySettings.AddAsync(settings);
        await _context.SaveChangesAsync();

        return settings;
    }

    public async Task<CompanySettings> UpdateAsync(UpdateCompanySettingsRequest request)
    {
        var settings = await GetAsync();

        settings.Update(
            request.StoreName,
            GenerateSlug(request.Slug),
            request.PrimaryColor,
            request.SecondaryColor,
            request.Whatsapp,
            request.Instagram,
            request.Address,
            request.HeroTitle,
            request.HeroSubtitle
        );

        await _context.SaveChangesAsync();

        return settings;
    }

    private static string GenerateSlug(string text)
    {
        return text
            .Trim()
            .ToLower()
            .Replace(" ", "-");
    }
}