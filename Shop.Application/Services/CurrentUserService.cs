using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Shop.Application.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _contextAccessor;

    public CurrentUserService(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public CurrentUser GetCurrentUser()
    {
        if (_contextAccessor?.HttpContext == null)
        {
            throw new InvalidOperationException("Context accessor should not be null to get CurrentUser");
        }
        
        var currentUserIdString = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(currentUserIdString, out var userGuid))
        {
            throw new InvalidOperationException($"User Guid provided in claims is an invalid guid: {currentUserIdString}");
        }

        var username = _contextAccessor.HttpContext.User.FindFirst(CustomClaimTypes.Username)?.Value;
        if (string.IsNullOrEmpty(username))
        {
            throw new InvalidOperationException($"User's username provided in claims is invalid: {username}");
        }
        
        var givenName = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.GivenName)?.Value;
        if (string.IsNullOrEmpty(givenName))
        {
            throw new InvalidOperationException($"User's given name provided in claims is invalid: {givenName}");
        }

        var middleName = _contextAccessor.HttpContext.User.FindFirst(CustomClaimTypes.MiddleName)?.Value;
        if (string.IsNullOrEmpty(middleName))
        {
            throw new InvalidOperationException($"User's surname provided in claims is invalid: {middleName}");
        }

        var birthdate = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.DateOfBirth)?.Value;
        if (birthdate == null || !DateTime.TryParse(birthdate, out var birthdateValue))
        {
            throw new InvalidOperationException($"User's birthdate provided in claims is invalid: {birthdate}");
        }

        var gender = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.Gender)?.Value;
        if (string.IsNullOrEmpty(gender))
        {
            throw new InvalidOperationException($"User's gender provided in claims is invalid: {gender}");
        }

        var email = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(email))
        {
            throw new InvalidOperationException($"User's email provided in claims is invalid: {email}");
        }

        return new CurrentUser
        {
            Id = userGuid,
            UserName = username,
            GivenName = givenName,
            MiddleName = middleName,
            Birthdate = birthdateValue,
            Gender = gender,
            Email = email
        };
    }

    private static class CustomClaimTypes
    {
        public const string Username = "cognito:username";
        public const string MiddleName = "middle_name";
    }
}