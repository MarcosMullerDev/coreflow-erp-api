using CoreFlow.Application.Auth.CurrentUser;
using CoreFlow.Application.Products.DTOs;
using CoreFlow.Application.Products.Repositories;
using CoreFlow.Domain.Entities;
using CoreFlow.Application.Common;

namespace CoreFlow.Application.Products.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly ICurrentUserService _currentUser;

    public ProductService(IProductRepository repository, ICurrentUserService currentUser)
    {
        _repository = repository;
        _currentUser = currentUser;
    }

    public async Task<Product> CreateAsync(CreateProductRequest request)
    {
        var product = new Product(
            _currentUser.CompanyId,
            request.Name,
            request.Description,
            request.Sku,
            request.CostPrice,
            request.SalePrice,
            request.StockQuantity
        );

        await _repository.AddAsync(product);
        await _repository.SaveChangesAsync();

        return product;
    }

    public async Task<PagedResult<Product>> GetAllAsync(PaginationRequest request)
    {
        var page = request.Page <= 0 ? 1 : request.Page;
        var pageSize = request.PageSize <= 0 ? 10 : request.PageSize;

        var result = await _repository.GetPagedByCompanyAsync(_currentUser.CompanyId, page, pageSize);

        return new PagedResult<Product>
        {
            Items = result.Items,
            TotalItems = result.TotalItems,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<Product?> UpdateAsync(Guid id, UpdateProductRequest request)
    {
        var product = await _repository.GetByIdAndCompanyForUpdateAsync(id, _currentUser.CompanyId);

        if (product is null)
            return null;

        product.Update(
            request.Name,
            request.Description,
            request.Sku,
            request.CostPrice,
            request.SalePrice,
            request.StockQuantity
        );

        await _repository.SaveChangesAsync();

        return product;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var product = await _repository.GetByIdAndCompanyForUpdateAsync(id, _currentUser.CompanyId);

        if (product is null)
            return false;

        product.Delete();

        await _repository.SaveChangesAsync();

        return true;
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _repository.GetByIdAndCompanyAsync(id, _currentUser.CompanyId);
    }
}