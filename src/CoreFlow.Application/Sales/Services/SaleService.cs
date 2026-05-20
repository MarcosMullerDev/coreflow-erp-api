using CoreFlow.Application.Auth.CurrentUser;
using CoreFlow.Application.Products.Repositories;
using CoreFlow.Application.Sales.DTOs;
using CoreFlow.Application.Sales.Repositories;
using CoreFlow.Domain.Entities;

namespace CoreFlow.Application.Sales.Services;

public class SaleService : ISaleService
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICurrentUserService _currentUser;

    public SaleService(
        ISaleRepository saleRepository,
        IProductRepository productRepository,
        ICurrentUserService currentUser)
    {
        _saleRepository = saleRepository;
        _productRepository = productRepository;
        _currentUser = currentUser;
    }

    public async Task<Sale> CreateAsync(CreateSaleRequest request)
    {
        if (request.Items.Count == 0)
            throw new Exception("Sale must have at least one item.");

        var saleItems = new List<SaleItem>();

        foreach (var item in request.Items)
        {
            var product = await _productRepository.GetByIdAndCompanyForUpdateAsync(
                item.ProductId,
                _currentUser.CompanyId
            );

            if (product is null)
                throw new Exception("Product not found.");

            product.DecreaseStock(item.Quantity);

            saleItems.Add(new SaleItem(
                product.Id,
                item.Quantity,
                product.SalePrice
            ));
        }

        var sale = new Sale(
            _currentUser.CompanyId,
            _currentUser.UserId,
            saleItems
        );

        await _saleRepository.AddAsync(sale);
        await _saleRepository.SaveChangesAsync();

        return sale;
    }

    public async Task<List<Sale>> GetAllAsync()
    {
        return await _saleRepository.GetAllByCompanyAsync(_currentUser.CompanyId);
    }
}