namespace Shop.Contracts.Requests;

public class CreateOrderRequest
{ 
    public required OrderItemDto[] Items { get; init; }
}

public class OrderItemDto
{
    public required Guid ProductId { get; init; }
    
    public required int Quantity { get; init; }
}