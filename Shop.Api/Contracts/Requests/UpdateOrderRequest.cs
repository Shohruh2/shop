using Shop.Api.Models;

namespace Shop.Api.Contracts.Requests;

public class UpdateOrderRequest
{
    public required DateTime DateTime { get; init; }
    
    public required Guid CustomerId { get; init; }
    
    public required List<OrderItem> OrderItems { get; init; }

    public required decimal TotalPrice { get; init; }
}