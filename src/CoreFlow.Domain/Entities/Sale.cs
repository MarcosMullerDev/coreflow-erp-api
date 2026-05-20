using CoreFlow.Domain.Common;
using CoreFlow.Domain.Enums;

namespace CoreFlow.Domain.Entities;

public class Sale : BaseEntity
{
    public Guid CompanyId { get; private set; }
    public Company Company { get; private set; } = null!;

    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;

    public decimal TotalAmount { get; private set; }
    public SaleStatus Status { get; private set; }

    public ICollection<SaleItem> Items { get; private set; } = new List<SaleItem>();

    private Sale() { }

    public Sale(Guid companyId, Guid userId, List<SaleItem> items)
    {
        CompanyId = companyId;
        UserId = userId;
        Items = items;
        TotalAmount = items.Sum(i => i.TotalPrice);
        Status = SaleStatus.Completed;
    }

    public void Cancel()
    {
        Status = SaleStatus.Canceled;
        SetUpdated();
    }
}