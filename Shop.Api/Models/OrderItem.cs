namespace Shop.Api.Models;

public class OrderItem
{
    public required Guid Id { get; set; }
    
    public required Guid ProductId { get; set; }
    
    public required decimal UnitPrice { get; set; }
    
    public required int Quantity { get; set; }
    
    public decimal TotalPrice => Quantity * UnitPrice;
}