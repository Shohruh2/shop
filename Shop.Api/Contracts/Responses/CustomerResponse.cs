namespace Shop.Api.Contracts.Responses;

public class CustomerResponse
{
    public required Guid Id { get; init; }
    
    public required string Name { get; init; }
    
    public required string Surname { get; init; }
    
    public required string Gender { get; init; }
    public required DateTime Birthday { get; init; }
    
    public required decimal Balance { get; init; }
}