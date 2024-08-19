using Shop.Domain.Products;

namespace Shop.Application.Services;

public interface IProductService
{
    Task<bool> CreateAsync(Product product, CancellationToken token = default);

    Task<Product?> GetByIdAsync(Guid id, CancellationToken token = default);

    Task<IEnumerable<Product>> GetAllAsync(CancellationToken token = default);

    Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);

    Task<Product?> UpdateAsync(Product product, CancellationToken token = default);
}