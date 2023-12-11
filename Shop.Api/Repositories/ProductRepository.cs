using Microsoft.EntityFrameworkCore;
using Shop.Api.Database;
using Shop.Api.Models;

namespace Shop.Api.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApiDbContext _dbContext;

    public ProductRepository(ApiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> CreateAsync(Product product, CancellationToken token = default)
    {
        _dbContext.Products.Add(product);
        var rowsAffected = await _dbContext.SaveChangesAsync(token);
        return rowsAffected > 0;
    }

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        var product = await _dbContext.Products.FindAsync(id, token);
        return product;
    }

    public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken token = default)
    {
        var products = await _dbContext.Products.ToListAsync(token);
        return products;
    }

    public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
    {
        var product = await _dbContext.Products.FindAsync(id);
        if (product != null)
        {
            _dbContext.Products.Remove(product);
            var rowsAffected = await _dbContext.SaveChangesAsync(token);
            return rowsAffected > 0;
        }
        
        return false;
    }

    public async Task<bool> UpdateAsync(Product product, CancellationToken token = default)
    {
        var existingProduct = await _dbContext.Products.FindAsync(product.Id, token);
        if (existingProduct == null)
        {
            return false;
        }

        existingProduct.Name = product.Name;
        existingProduct.Description = product.Description;
        existingProduct.Price = product.Price;
        existingProduct.Quantity = product.Quantity;

        var rowsAffected = await _dbContext.SaveChangesAsync(token);
        return rowsAffected > 0;
    }

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default)
    {
        var count = await _dbContext.Products.CountAsync(x => x.Id == id, token);
        return count > 0;
    }
}