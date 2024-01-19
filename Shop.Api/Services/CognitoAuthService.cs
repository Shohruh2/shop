using System.Net;
using System.Security.Cryptography;
using System.Text;
using Amazon.CognitoIdentityProvider;
using Shop.Api.Contracts.Requests;
using Shop.Api.Mapping;

namespace Shop.Api.Services;

public class CognitoAuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly IAmazonCognitoIdentityProvider _cognitoIdentityProvider;
    private readonly ICustomerService _customerService;

    public CognitoAuthService(IConfiguration configuration, IAmazonCognitoIdentityProvider cognitoIdentityProvider, ICustomerService customerService)
    {
        _configuration = configuration;
        _cognitoIdentityProvider = cognitoIdentityProvider;
        _customerService = customerService;
    }

    public async Task<bool> Register(RegistrationRequest registrationRequest, CancellationToken token = default)
    {
        var clientId = _configuration["AWS:ClientId"];
        var clientSecret = _configuration["AWS:ClientSecret"];
        if (clientId != null && clientSecret != null)
        {
            var secretHash = GenerateSecretHash(registrationRequest.UserName, clientId, clientSecret);
            var signUpRequest = registrationRequest.MapToSignUpRequest(clientId, secretHash, clientSecret);
        
            var result = await _cognitoIdentityProvider.SignUpAsync(signUpRequest, token);
            if (result.HttpStatusCode != HttpStatusCode.OK) 
            { 
                return false;
            }

            await _customerService.CreateAsync(new CreateCustomerRequest
            {
                Id = Guid.Parse(result.UserSub),
                Name = registrationRequest.FirstName,
                Surname = registrationRequest.LastName,
                Gender = registrationRequest.Gender,
                Birthday = registrationRequest.Birthday,
            }, token);
            
            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task<bool> ConfirmRegistration(ConfirmRegistrationRequest confirmRegistrationRequest, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> Login(LoginRequest loginRequest, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public async Task<string> RefreshToken(RefreshTokenRequest refreshTokenRequest, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public string GenerateSecretHash(string username, string clientId, string clientSecret)
    {
        var message = Encoding.UTF8.GetBytes(username + clientId);
        using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(clientSecret)))
        {
            var hash = hmac.ComputeHash(message);
            return Convert.ToBase64String(hash);
        }
    }
}