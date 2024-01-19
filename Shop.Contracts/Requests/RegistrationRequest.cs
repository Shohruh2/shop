namespace Shop.Contracts.Requests;

public class RegistrationRequest
{
    public required string FirstName { get; init; }
    
    public required string LastName { get; init; }
    
    public required string Email { get; init; }
    
    public required string Gender { get; init; }
    
    public required DateTime Birthday { get; init; }
    
    public required string UserName { get; init; }
    
    public required string Password { get; init; }
}