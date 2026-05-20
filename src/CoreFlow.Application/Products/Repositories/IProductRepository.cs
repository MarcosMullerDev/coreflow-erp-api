using CoreFlow.Domain.Entities;

namespace CoreFlow.Application.Products.Repositories;

public interface IProductRepository
{
    Task AddAsync(Product product);
    Task<(List<Product> Items, int TotalItems)> GetPagedByCompanyAsync(Guid companyId, int page, int pageSize);
    Task<Product?> GetByIdAndCompanyForUpdateAsync(Guid id, Guid companyId);
    Task<Product?> GetByIdAndCompanyAsync(Guid id, Guid companyId);
    Task SaveChangesAsync();
}