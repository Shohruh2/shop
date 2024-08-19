namespace Shop.Contracts.Requests.AuthRequests;

public class ConfirmRegistrationRequest
{
    public required string UserName { get; init; }
    
    public required string ConfrimationCode { get; init; }
}