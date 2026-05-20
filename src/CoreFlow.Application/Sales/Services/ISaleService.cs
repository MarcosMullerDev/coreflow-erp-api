using CoreFlow.Application.Sales.DTOs;
using CoreFlow.Domain.Entities;

namespace CoreFlow.Application.Sales.Services;

public interface ISaleService
{
    Task<Sale> CreateAsync(CreateSaleRequest request);
    Task<List<Sale>> GetAllAsync();
}