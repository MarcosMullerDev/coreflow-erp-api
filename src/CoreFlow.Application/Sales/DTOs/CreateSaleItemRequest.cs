namespace CoreFlow.Application.Sales.DTOs;

public class CreateSaleItemRequest
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}