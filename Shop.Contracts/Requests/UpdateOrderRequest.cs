using Shop.Contracts.Responses;

namespace Shop.Contracts.Requests;

public class UpdateOrderRequest
{
    public required DateTime DateTime { get; init; }
    
    public required Guid CustomerId { get; init; }
    
    public required List<OrderResponseItemDto> OrderItems { get; init; }

    public required decimal TotalPrice { get; init; }
}