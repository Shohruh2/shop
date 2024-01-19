using System.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Shop.Api.Database;
using Shop.Api.Repositories;
using Shop.Api.Services;

namespace Shop.Api;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
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