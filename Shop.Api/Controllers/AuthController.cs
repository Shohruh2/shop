using System.Net;
using Amazon.CognitoIdentityProvider;
using Amazon.Runtime.Internal;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Services;
using Shop.Contracts.Requests;
using Shop.Contracts.Requests.AuthRequests;
using Shop.Contracts.Responses.AuthResponses;
using Shop.Contracts.Responses.StandartResponse;
using ResponseError = Shop.Contracts.Responses.StandartResponse.ResponseError;

namespace Shop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest model, CancellationToken token)
        {
            var register = await _authService.Register(model, token);
            if (!register)
            {
                return Ok("User registered successfully");
            }

            return BadRequest($"Registration failed");
        }

        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmRegistration([FromBody] ConfirmRegistrationRequest model,
            CancellationToken token)
        {
            var confirm = await _authService.ConfirmRegistration(model, token);
            if (!confirm)
            {
                return Ok("User confirmed successfully");
            }

            return BadRequest("Confirmation failed");
        }

        [HttpPost("login")]
        public async Task<ActionResult<CustomResponse<AuthLoginResponse>>> Login([FromBody] LoginRequest model, CancellationToken token)
        {
            var tokens = await _authService.Login(model, token);
            if (tokens == null)
            {
                var errorResponse = CustomResponse<AuthLoginResponse>.CreateErrorResponse(new ResponseError
                {
                    Message = "Login or password incorrect",
                    Code = HttpStatusCode.BadRequest.ToString()
                });
                return errorResponse;
            }

            var successResponse = CustomResponse<AuthLoginResponse>.CreateSuccessResponse(tokens);

            return successResponse;
        }


        [HttpPost("refresh")]
        public async Task<ActionResult<CustomResponse<AuthRefreshResponse>>> RefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest, CancellationToken token)
        {
            var refreshIdToken = await _authService.RefreshToken(refreshTokenRequest, token);
            if (refreshIdToken == null)
            {
                var errorResponse = CustomResponse<AuthRefreshResponse>.CreateErrorResponse(new ResponseError
                {
                    Message = "Username or refresh token is incorrect",
                    Code = HttpStatusCode.BadGateway.ToString()
                });
                return errorResponse;
            }

            var successResponse = CustomResponse<AuthRefreshResponse>.CreateSuccessResponse(refreshIdToken);
            return successResponse;
        }
    }
}