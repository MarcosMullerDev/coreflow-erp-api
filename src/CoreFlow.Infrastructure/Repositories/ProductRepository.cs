using CoreFlow.Application.Products.Repositories;
using CoreFlow.Domain.Entities;
using CoreFlow.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CoreFlow.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
    }

    public async Task<Product?> GetByIdAndCompanyAsync(Guid id, Guid companyId)
    {
        return await _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id && p.CompanyId == companyId && !p.IsDeleted);
    }
    public async Task<(List<Product> Items, int TotalItems)> GetPagedByCompanyAsync(Guid companyId, int page, int pageSize)
    {
        var query = _context.Products
            .AsNoTracking()
            .Where(p => p.CompanyId == companyId && !p.IsDeleted);

        var totalItems = await query.CountAsync();

        var items = await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalItems);
    }

    public async Task<Product?> GetByIdAndCompanyForUpdateAsync(Guid id, Guid companyId)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id && p.CompanyId == companyId && !p.IsDeleted);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}