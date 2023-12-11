using Shop.Api.Models;

namespace Shop.Api.Contracts.Responses;

public class OrderResponse
{
    public required Guid Id { get; init; }
    
    public required DateTime DateTime { get; init; }

    public required List<OrderItem> OrderItems { get; init; }
}