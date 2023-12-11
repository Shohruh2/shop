using Shop.Api.Models;

namespace Shop.Api.Services;

public interface ICurrentUserService
{
    CurrentUser GetCurrentUser();
}