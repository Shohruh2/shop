namespace Shop.Api.Settings;

public class AuthCognitoSettings
{
    private const string SectionName = "AuthCognito";
    
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
}