namespace Shop.Contracts.Requests;

public class RefreshTokenRequest
{
    public required string UserName { get; set; }
    
    public required string RefreshToken { get; set; }
}