namespace Shop.Contracts.Requests;

public class CreateCustomerRequest
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    
    public required string Surname { get; init; }
    
    public required string Gender { get; init; }
    
    public required DateTime Birthday { get; init; }
}