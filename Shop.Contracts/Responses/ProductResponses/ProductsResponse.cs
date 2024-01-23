namespace Shop.Contracts.Responses.ProductResponses;

public class ProductsResponse
{
    public required IEnumerable<ProductResponse> Items { get; init; } = Enumerable.Empty<ProductResponse>();
}