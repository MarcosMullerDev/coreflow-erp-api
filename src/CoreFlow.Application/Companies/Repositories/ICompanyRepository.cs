using CoreFlow.Domain.Entities;

namespace CoreFlow.Application.Companies.Repositories;

public interface ICompanyRepository
{
    Task AddAsync(Company company);
    Task<List<Company>> GetAllAsync();
    Task<Company?> GetByIdAsync(Guid id);
    Task SaveChangesAsync();
}