using CoreFlow.Application.CompanySettings.DTOs;
using CoreFlow.Domain.Entities;

namespace CoreFlow.Application.CompanySettings.Services;

public interface ICompanySettingsService
{
    Task<CoreFlow.Domain.Entities.CompanySettings> GetAsync();
    Task<CoreFlow.Domain.Entities.CompanySettings> UpdateAsync(UpdateCompanySettingsRequest request);
}