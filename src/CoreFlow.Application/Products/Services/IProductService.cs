using CoreFlow.Application.Common;
using CoreFlow.Application.Products.DTOs;
using CoreFlow.Domain.Entities;

namespace CoreFlow.Application.Products.Services;

public interface IProductService
{
    Task<Product> CreateAsync(CreateProductRequest request);
    Task<PagedResult<Product>> GetAllAsync(PaginationRequest request);
    Task<Product?> GetByIdAsync(Guid id);
    Task<Product?> UpdateAsync(Guid id, UpdateProductRequest request);
    Task<bool> DeleteAsync(Guid id);
}