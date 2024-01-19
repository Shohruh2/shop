using Shop.Api.Contracts.Requests;

namespace Shop.Api.Services;

public interface IAuthService
{
    Task<bool> Register(RegistrationRequest registrationRequest, CancellationToken token = default);

    Task<bool> ConfirmRegistration(ConfirmRegistrationRequest confirmRegistrationRequest,
        CancellationToken token = default);

    Task<bool> Login(LoginRequest loginRequest, CancellationToken token = default);

    Task<string> RefreshToken(RefreshTokenRequest refreshTokenRequest, CancellationToken token = default);

    string GenerateSecretHash(string username, string clientId, string clientSecret);
}