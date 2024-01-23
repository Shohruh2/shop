using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Shop.Contracts.Requests;
using Shop.Contracts.Requests.AuthRequests;

namespace Shop.Infrastructure.Cognito;

public static class AuthMapper
{
    public static SignUpRequest MapToSignUpRequest(this RegistrationRequest request, string clientId, string secretHash)
    {
        return new SignUpRequest
        {
            ClientId = clientId,
            SecretHash = secretHash,
            Username = request.UserName,
            Password = request.Password,
            UserAttributes = new List<AttributeType>
            {
                new AttributeType { Name = "given_name", Value = request.FirstName},
                new AttributeType { Name = "middle_name", Value = request.LastName},
                new AttributeType { Name = "email", Value = request.Email},
                new AttributeType { Name = "gender", Value = request.Gender},
                new AttributeType { Name = "birthdate", Value = request.Birthday.ToString("dd/MM/yyyy")},
            }
        };
    }

    public static ConfirmSignUpRequest MapToSignUpRequest(this ConfirmRegistrationRequest request, string? clientId,
        string secretHash)
    {
        return new ConfirmSignUpRequest
        {
            ClientId = clientId,
            Username = request.UserName,
            ConfirmationCode = request.ConfrimationCode,
            SecretHash = secretHash
        };
    }

    public static AdminInitiateAuthRequest MapToInitiateAuthRequest(this LoginRequest loginRequest, string? clientId, string secretHash)
    {
        return new AdminInitiateAuthRequest
        {
            UserPoolId = "eu-north-1_2sVtDUSZu",
            ClientId = clientId,
            AuthFlow = AuthFlowType.ADMIN_NO_SRP_AUTH,
            AuthParameters = new Dictionary<string, string>
            {
                {"USERNAME", loginRequest.UserName},
                {"PASSWORD", loginRequest.Password},
                {"SECRET_HASH", secretHash}
            } 
        };
    }
}