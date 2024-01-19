namespace Shop.Contracts.Responses;

public class CustomersResponse
{
    public required IEnumerable<CustomerResponse> Items { get; init; } = Enumerable.Empty<CustomerResponse>();
}