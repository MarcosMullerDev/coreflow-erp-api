using CoreFlow.Application.Companies.DTOs;
using CoreFlow.Application.Companies.Repositories;
using CoreFlow.Domain.Entities;

namespace CoreFlow.Application.Companies.Services;

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _repository;

    public CompanyService(ICompanyRepository repository)
    {
        _repository = repository;
    }

    public async Task<Company> CreateAsync(CreateCompanyRequest request)
    {
        var company = new Company(
            request.Name,
            request.Document,
            request.Email,
            request.Phone
        );

        await _repository.AddAsync(company);
        await _repository.SaveChangesAsync();

        return company;
    }

    public async Task<List<Company>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Company?> GetByIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }
}