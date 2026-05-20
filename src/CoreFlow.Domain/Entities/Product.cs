using CoreFlow.Domain.Common;

namespace CoreFlow.Domain.Entities;

public class Product : BaseEntity
{
    public Guid CompanyId { get; private set; }
    public Company Company { get; private set; } = null!;

    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public string Sku { get; private set; } = string.Empty;
    public decimal CostPrice { get; private set; }
    public decimal SalePrice { get; private set; }
    public int StockQuantity { get; private set; }
    public bool IsActive { get; private set; }

    private Product() { }

    public Product(Guid companyId, string name, string? description, string sku, decimal costPrice, decimal salePrice, int stockQuantity)
    {
        CompanyId = companyId;
        Name = name;
        Description = description;
        Sku = sku;
        CostPrice = costPrice;
        SalePrice = salePrice;
        StockQuantity = stockQuantity;
        IsActive = true;
    }

    public void Update(string name, string? description, string sku, decimal costPrice, decimal salePrice, int stockQuantity)
    {
        Name = name;
        Description = description;
        Sku = sku;
        CostPrice = costPrice;
        SalePrice = salePrice;
        StockQuantity = stockQuantity;

        SetUpdated();
    }

    public void Delete()
    {
        MarkAsDeleted();
    }

    public void DecreaseStock(int quantity)
    {
        if (quantity <= 0)
            throw new Exception("Quantity must be greater than zero.");

        if (StockQuantity < quantity)
            throw new Exception("Insufficient stock.");

        StockQuantity -= quantity;
        SetUpdated();
    }
}