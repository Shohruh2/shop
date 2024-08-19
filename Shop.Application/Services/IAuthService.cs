using Shop.Contracts.Requests;
using Shop.Contracts.Requests.AuthRequests;
using Shop.Contracts.Responses.AuthResponses;

namespace Shop.Application.Services;

public interface IAuthService
{
    Task<bool> Register(RegistrationRequest registrationRequest, CancellationToken token = default);

    Task<bool> ConfirmRegistration(ConfirmRegistrationRequest confirmRegistrationRequest,
        CancellationToken token = default);

    Task<AuthLoginResponse?> Login(LoginRequest loginRequest, CancellationToken token = default);

    Task<AuthRefreshResponse?> RefreshToken(RefreshTokenRequest refreshTokenRequest, CancellationToken token = default);

    string GenerateSecretHash(string username, string clientId, string clientSecret);
}