using Amazon.CognitoIdentityProvider;
using Microsoft.AspNetCore.Mvc;
using Shop.Api.Contracts.Requests;
using Shop.Api.Services;

namespace Shop.Api.Controllers
{ 
    [Route("api/[controller]")] 
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAmazonCognitoIdentityProvider cognitoIdentityProvider, ICustomerService customerService, IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest model, CancellationToken token)
        {
            var register = await _authService.Register(model, token);
            if (register != false)
            {
                return Ok("User registered successfully");
            }
            else
            {
                return BadRequest($"Registration failed");
            }
        }
        
        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmRegistration([FromBody] ConfirmRegistrationRequest model, CancellationToken token)
        {
            var confirm = await _authService.ConfirmRegistration(model, token);
            if (confirm != false)
            {
                return Ok("User confirmed successfully");
            }
            else
            {
                return BadRequest("Confirmation failed");
            }
        }
        
        //
        // [HttpPost("login")]
        // public async Task<IActionResult> Login([FromBody] LoginRequest model)
        // {
        //     var initiateAuthRequest = new AdminInitiateAuthRequest
        //     {
        //         UserPoolId = "eu-north-1_2sVtDUSZu",
        //         ClientId = _clientId,
        //         AuthFlow = AuthFlowType.ADMIN_NO_SRP_AUTH,
        //         AuthParameters = new Dictionary<string, string>
        //         {
        //             {"USERNAME", model.UserName},
        //             {"PASSWORD", model.Password},
        //             {"SECRET_HASH", GenerateSecretHash(model.UserName, _clientId, _clientSecret)}
        //         } 
        //     };
        //
        //     try
        //     {
        //         var authResponse = await _cognitoIdentityProvider.AdminInitiateAuthAsync(initiateAuthRequest);
        //         
        //
        //         string accessToken = authResponse.AuthenticationResult.AccessToken;
        //         var idToken = authResponse.AuthenticationResult.IdToken;
        //         string refreshToken = authResponse.AuthenticationResult.RefreshToken;
        //
        //         if (authResponse.HttpStatusCode != HttpStatusCode.OK)
        //         {
        //             return BadRequest("Authentication failed");
        //         }
        //
        //     // В authResponse.AccessToken содержится токен доступа, который может быть использован для аутентификации пользователя
        //     // В authResponse.IdToken содержится ID-токен, который может содержать дополнительную информацию о пользователе
        //
        //         return Ok(new { Idtoken = idToken, AccessToken = accessToken, RefreshToken = refreshToken, Message = "Login successful" });
        //     }
        //     catch (AmazonCognitoIdentityProviderException e)
        //     { 
        //         return BadRequest($"Authentication failed: {e.Message}");
        //     }
        // }
        // [HttpPost("refresh")]
        // public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest)
        // {
        //     var adminInitiateAuthRequest = new AdminInitiateAuthRequest
        //     {
        //         UserPoolId = "eu-north-1_2sVtDUSZu",
        //         ClientId = _clientId,
        //         AuthFlow = AuthFlowType.REFRESH_TOKEN,
        //         AuthParameters = new Dictionary<string, string>
        //         {
        //             {"REFRESH_TOKEN", refreshTokenRequest.RefreshToken},
        //             {"SECRET_HASH", GenerateSecretHash(refreshTokenRequest.UserName, _clientId, _clientSecret)}
        //         } 
        //     };
        //
        //     try
        //     {
        //         var authResponse = await _cognitoIdentityProvider.AdminInitiateAuthAsync(adminInitiateAuthRequest);
        //
        //         string newIdToken = authResponse.AuthenticationResult.IdToken;
        //
        //         if (authResponse.HttpStatusCode != HttpStatusCode.OK)
        //         {
        //             return BadRequest("Token refresh failed");
        //         }
        //
        //         return Ok(new { NewIdToken = newIdToken, Message = "Token refresh successful" });
        //     }
        //     catch (AmazonCognitoIdentityProviderException e)
        //     {
        //         return BadRequest($"Token refresh failed: {e.Message}");
        //     }
        // }
        //
        // private string GenerateSecretHash(string username, string clientId, string clientSecret)
        // {
        //     var message = Encoding.UTF8.GetBytes(username + clientId);
        //     using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(clientSecret)))
        //     {
        //         var hash = hmac.ComputeHash(message);
        //         return Convert.ToBase64String(hash);
        //     }
        // }
    }
}