using Amazon.CognitoIdentityProvider;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Services;
using Shop.Contracts.Requests;
using Shop.Contracts.Requests.AuthRequests;

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
            return BadRequest($"Registration failed");
        }
        
        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmRegistration([FromBody] ConfirmRegistrationRequest model, CancellationToken token)
        {
            var confirm = await _authService.ConfirmRegistration(model, token);
            if (confirm != false)
            {
                return Ok("User confirmed successfully");
            }
            
            return BadRequest("Confirmation failed");
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model, CancellationToken token)
        {
            var accessToken = await _authService.Login(model, token);
            if (accessToken == null)
            {
                return BadRequest("Login or password incorrect");
            }
            
            return Ok(new { AccessToken = accessToken, Message = "Login successful" });
           
        }
        
        
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