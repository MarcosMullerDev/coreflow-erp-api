using CoreFlow.Application.Companies.DTOs;
using CoreFlow.Domain.Entities;

namespace CoreFlow.Application.Companies.Services;

public interface ICompanyService
{
    Task<Company> CreateAsync(CreateCompanyRequest request);
    Task<List<Company>> GetAllAsync();
    Task<Company?> GetByIdAsync(Guid id);
}