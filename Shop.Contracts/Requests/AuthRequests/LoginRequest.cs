namespace Shop.Contracts.Requests.AuthRequests;

public class LoginRequest
{
    public required string UserName { get; init; }
    
    public required string Password { get; init; }
}