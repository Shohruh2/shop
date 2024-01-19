using Microsoft.EntityFrameworkCore;
using Shop.Domain.Customers;
using Shop.Domain.Orders;
using Shop.Domain.Products;

namespace Shop.Infrastructure.Persistence;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }
    
    public DbSet<Product> Products { get; set; }
    
    public DbSet<Customer> Customers { get; set; }
    
    public DbSet<Order> Orders { get; set; }
}