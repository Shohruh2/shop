using FluentValidation;
using Shop.Api.Middleware;
using Shop.Application.Services;
using Shop.Domain.Customers;
using Shop.Domain.Orders;
using Shop.Domain.Products;
using Shop.Infrastructure.Cognito;
using Shop.Infrastructure.Persistence.Repositories;

namespace Shop.Api;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddTransient<CustomExceptionMiddleware>();
        services.AddTransient<IProductRepository, ProductRepository>();
        services.AddTransient<IProductService, ProductService>();
        services.AddTransient<ICustomerRepository, CustomerRepository>();
        services.AddTransient<ICustomerService, CustomerService>();
        services.AddTransient<IOrderRepository, OrderRepository>();
        services.AddTransient<IOrderService, OrderService>();
        services.AddTransient<IAuthService, CognitoAuthService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddValidatorsFromAssemblyContaining<IApplicationMarker>(ServiceLifetime.Singleton);
        return services;
    }
}