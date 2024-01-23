namespace Shop.Contracts.Responses.AuthResponses;

public class AuthLoginResponse
{
    public string IdToken { get; set; }

    public string AccessToken { get; set; }

    public string RefreshToken { get; set; }
}