namespace Shop.Contracts.Responses.CustomerResponses;

public class CustomersResponse
{
    public required IEnumerable<CustomerResponse> Items { get; init; } = Enumerable.Empty<CustomerResponse>();
}