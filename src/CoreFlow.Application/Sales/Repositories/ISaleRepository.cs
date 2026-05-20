using CoreFlow.Domain.Entities;

namespace CoreFlow.Application.Sales.Repositories;

public interface ISaleRepository
{
    Task AddAsync(Sale sale);
    Task<List<Sale>> GetAllByCompanyAsync(Guid companyId);
    Task SaveChangesAsync();
}