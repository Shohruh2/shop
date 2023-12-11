using Microsoft.EntityFrameworkCore;
using Shop.Api.Models;

namespace Shop.Api.Database;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }
    
    public DbSet<Product> Products { get; set; }
    
    public DbSet<Customer> Customers { get; set; }
    
    public DbSet<Order> Orders { get; set; }
}