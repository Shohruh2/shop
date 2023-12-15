using System.Net;
using System.Security.Cryptography;
using System.Text;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shop.Api.Contracts.Requests;
using Shop.Api.Services;
using RefreshTokenRequest = Shop.Api.Contracts.Requests.RefreshTokenRequest;

namespace Shop.Api.Controllers
{ 
    [Route("api/[controller]")] 
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAmazonCognitoIdentityProvider _cognitoIdentityProvider;
        private readonly ICustomerService _customerService;
        private readonly string _clientId = "t4maqhd858lfj4jtf3ib4kptv";
        private readonly string _clientSecret = "8ognl84i7v44l2oro6kru0npp667poqvgpv89qvtuunk9gth3mv";

        public AuthController(IAmazonCognitoIdentityProvider cognitoIdentityProvider, ICustomerService customerService)
        {
            _cognitoIdentityProvider = cognitoIdentityProvider ?? throw new ArgumentNullException(nameof(cognitoIdentityProvider));
            _customerService = customerService ?? throw new ArgumentNullException(nameof(customerService));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest model)
        {
            var request = new SignUpRequest
            {
                ClientId = _clientId,
                SecretHash = GenerateSecretHash(model.UserName, _clientId, _clientSecret),
                Username = model.UserName,
                Password = model.Password,
                UserAttributes = new List<AttributeType>
                {
                    new AttributeType { Name = "given_name", Value = model.FirstName},
                    new AttributeType { Name = "middle_name", Value = model.LastName},
                    new AttributeType { Name = "email", Value = model.Email},
                    new AttributeType { Name = "gender", Value = model.Gender},
                    new AttributeType { Name = "birthdate", Value = model.Birthday.ToString("dd/MM/yyyy")},
                }
            };

            try
            {
                var result = await _cognitoIdentityProvider.SignUpAsync(request);
                if (result.HttpStatusCode != HttpStatusCode.OK)
                {
                    return BadRequest();
                }

                await _customerService.CreateAsync(new CreateCustomerRequest
                {
                    Id = Guid.Parse(result.UserSub),
                    Name = model.FirstName,
                    Surname = model.LastName,
                    Gender = model.Gender,
                    Birthday = model.Birthday,
                });

                return Ok("User registered successfully");
            }
            catch (AmazonCognitoIdentityProviderException e)
            {
                return BadRequest($"Registration failed: {e.Message}");
            }
        }

        // ... (остальные методы)
        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmRegistration([FromBody] ConfirmRegistrationRequest model)
        {
            var confirmRequest = new ConfirmSignUpRequest
            {
                ClientId = "t4maqhd858lfj4jtf3ib4kptv",
                Username = model.UserName,
                ConfirmationCode = model.ConfrimationCode,
                SecretHash = GenerateSecretHash(model.UserName, _clientId, _clientSecret)
            };
        
            try
            {
                var confirmResult = await _cognitoIdentityProvider.ConfirmSignUpAsync(confirmRequest);
                if (confirmResult.HttpStatusCode != HttpStatusCode.OK) 
                {
                    return BadRequest("Confirmation failed");
                }

                return Ok("User registration confirmed successfully");
            }
            catch (AmazonCognitoIdentityProviderException e)
            {
                return BadRequest($"Confirmation failed: {e.Message}");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            var initiateAuthRequest = new AdminInitiateAuthRequest
            {
                UserPoolId = "eu-north-1_2sVtDUSZu",
                ClientId = _clientId,
                AuthFlow = AuthFlowType.ADMIN_NO_SRP_AUTH,
                AuthParameters = new Dictionary<string, string>
                {
                    {"USERNAME", model.UserName},
                    {"PASSWORD", model.Password},
                    {"SECRET_HASH", GenerateSecretHash(model.UserName, _clientId, _clientSecret)}
                } 
            };
        
            try
            {
                var authResponse = await _cognitoIdentityProvider.AdminInitiateAuthAsync(initiateAuthRequest);
                

                string accessToken = authResponse.AuthenticationResult.AccessToken;
                var idToken = authResponse.AuthenticationResult.IdToken;
                string refreshToken = authResponse.AuthenticationResult.RefreshToken;

                if (authResponse.HttpStatusCode != HttpStatusCode.OK)
                {
                    return BadRequest("Authentication failed");
                }

            // В authResponse.AccessToken содержится токен доступа, который может быть использован для аутентификации пользователя
            // В authResponse.IdToken содержится ID-токен, который может содержать дополнительную информацию о пользователе

                return Ok(new { Idtoken = idToken, AccessToken = accessToken, RefreshToken = refreshToken, Message = "Login successful" });
            }
            catch (AmazonCognitoIdentityProviderException e)
            { 
                return BadRequest($"Authentication failed: {e.Message}");
            }
        }
        
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest)
        {
            var adminInitiateAuthRequest = new AdminInitiateAuthRequest
            {
                UserPoolId = "eu-north-1_2sVtDUSZu",
                ClientId = _clientId,
                AuthFlow = AuthFlowType.REFRESH_TOKEN,
                AuthParameters = new Dictionary<string, string>
                {
                    {"REFRESH_TOKEN", refreshTokenRequest.RefreshToken},
                    {"SECRET_HASH", GenerateSecretHash(refreshTokenRequest.UserName, _clientId, _clientSecret)}
                } 
            };

            try
            {
                var authResponse = await _cognitoIdentityProvider.AdminInitiateAuthAsync(adminInitiateAuthRequest);

                string newIdToken = authResponse.AuthenticationResult.IdToken;

                if (authResponse.HttpStatusCode != HttpStatusCode.OK)
                {
                    return BadRequest("Token refresh failed");
                }

                return Ok(new { NewIdToken = newIdToken, Message = "Token refresh successful" });
            }
            catch (AmazonCognitoIdentityProviderException e)
            {
                return BadRequest($"Token refresh failed: {e.Message}");
            }
        }
        
        private string GenerateSecretHash(string username, string clientId, string clientSecret)
        {
            var message = Encoding.UTF8.GetBytes(username + clientId);
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(clientSecret)))
            {
                var hash = hmac.ComputeHash(message);
                return Convert.ToBase64String(hash);
            }
        }
    }
}