namespace Shop.Api.Contracts.Requests;

public class UpdateCustomerRequest
{
    public required string Name { get; init; }
    
    public required string Surname { get; init; }
    
    public required string Gender { get; init; }
    
    public required DateTime Birthday { get; init; }
}