using Amazon.CognitoIdentityProvider.Model;
using Shop.Api.Contracts.Requests;

namespace Shop.Api.Mapping;

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
}