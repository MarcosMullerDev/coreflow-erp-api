namespace CoreFlow.Application.Sales.DTOs;

public class CreateSaleRequest
{
    public List<CreateSaleItemRequest> Items { get; set; } = new();
}