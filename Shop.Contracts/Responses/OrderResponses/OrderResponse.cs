﻿namespace Shop.Contracts.Responses.OrderResponses;

public class OrderResponse
{
    public required Guid Id { get; init; }
    
    public required DateTime DateTime { get; init; }

    public required List<OrderResponseItemDto> OrderItems { get; init; }
}