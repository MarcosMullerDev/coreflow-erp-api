using CoreFlow.Application.Auth.CurrentUser;
using CoreFlow.Application.Companies.DTOs;
using CoreFlow.Application.Companies.Repositories;
using CoreFlow.Domain.Entities;

namespace CoreFlow.Application.Companies.Services;

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _repository;
    private readonly ICurrentUserService _currentUser;

    public CompanyService(
        ICompanyRepository repository,
        ICurrentUserService currentUser)
    {
        _repository = repository;
        _currentUser = currentUser;
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
        var company = await _repository.GetByIdAsync(_currentUser.CompanyId);

        return company is null
            ? new List<Company>()
            : new List<Company> { company };
    }

    public async Task<Company?> GetByIdAsync(Guid id)
    {
        if (id != _currentUser.CompanyId)
            return null;

        return await _repository.GetByIdAsync(id);
    }
}