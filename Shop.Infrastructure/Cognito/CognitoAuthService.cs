using System.Net;
using System.Security.Cryptography;
using System.Text;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Shop.Application.Mapping;
using Shop.Application.Services;
using Shop.Contracts.Requests;
using Shop.Contracts.Requests.AuthRequests;
using Shop.Contracts.Requests.CustomerRequests;
using Shop.Contracts.Responses.AuthResponses;

namespace Shop.Infrastructure.Cognito;

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
            var signUpRequest = registrationRequest.MapToSignUpRequest(clientId, secretHash);

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

        return false;
        
    }

    public async Task<bool> ConfirmRegistration(ConfirmRegistrationRequest confirmRegistrationRequest, CancellationToken token = default)
    {
        var clientId = _configuration["AWS:ClientId"];
        var clientSecret = _configuration["AWS:ClientSecret"];
        if (clientId != null && clientSecret != null) 
        {
            var secretHash = GenerateSecretHash(confirmRegistrationRequest.UserName, clientId, clientSecret);
            var confirmCode = confirmRegistrationRequest.MapToSignUpRequest(clientId, secretHash);
        
            var confirmResult = await _cognitoIdentityProvider.ConfirmSignUpAsync(confirmCode, token);
            if (confirmResult.HttpStatusCode != HttpStatusCode.OK)
            {
                return false;
            }

            return true;
        }
        
        return false;
    }

    public async Task<AuthLoginResponse?> Login(LoginRequest loginRequest, CancellationToken token = default)
    {
        var clientId = _configuration["AWS:ClientId"];
        var clientSecret = _configuration["AWS:ClientSecret"];
        if (clientId != null && clientSecret != null)
        {
            var secretHash = GenerateSecretHash(loginRequest.UserName, clientId, clientSecret);
            var login = loginRequest.MapToInitiateAuthRequest(clientId, secretHash);
        
            try
            {
                var authResponse = await _cognitoIdentityProvider.AdminInitiateAuthAsync(login, token);
                var refreshToken = authResponse.AuthenticationResult.RefreshToken;
                var idToken = authResponse.AuthenticationResult.IdToken;
                var accessToken = authResponse.AuthenticationResult.AccessToken;
                if (authResponse.HttpStatusCode == HttpStatusCode.OK)
                {
                    return new AuthLoginResponse
                    {
                        AccessToken = accessToken,
                        IdToken = idToken,
                        RefreshToken = refreshToken
                    };
                }

                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        return null;
    }

    public async Task<AuthRefreshResponse?> RefreshToken(RefreshTokenRequest refreshTokenRequest, CancellationToken token = default)
    {
        var clientId = _configuration["AWS:ClientId"];
        var clientSecret = _configuration["AWS:ClientSecret"];
        if (clientId != null && clientSecret != null)
        {
            var secretHash = GenerateSecretHash(refreshTokenRequest.UserName, clientId, clientSecret);
            var adminInitiateRequest = refreshTokenRequest.MapToInitiateAuthRequest(clientId, secretHash);
            
            try
            {
                var authResponse = await _cognitoIdentityProvider.AdminInitiateAuthAsync(adminInitiateRequest, token);

                string newIdToken = authResponse.AuthenticationResult.IdToken;

                if (authResponse.HttpStatusCode != HttpStatusCode.OK)
                {
                    return null;
                }

                return new AuthRefreshResponse
                {
                    IdToken = newIdToken
                };
            }
            catch (Exception e)
            {
                return null;
            }
        }

        return null;
    }

    public string GenerateSecretHash(string username, string clientId, string clientSecret)
    {
        var message = Encoding.UTF8.GetBytes(username + clientId);
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(clientSecret));
        var hash = hmac.ComputeHash(message);
        return Convert.ToBase64String(hash);
    }
}