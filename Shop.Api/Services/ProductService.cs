using FluentValidation;
using Shop.Api.Models;
using Shop.Api.Repositories;

namespace Shop.Api.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IValidator<Product> _productValidator;

    public ProductService(IProductRepository productRepository, IValidator<Product> productValidator)
    {
        _productRepository = productRepository;
        _productValidator = productValidator;
    }

    public async Task<bool> CreateAsync(Product product, CancellationToken token = default)
    {
        await _productValidator.ValidateAndThrowAsync(product, cancellationToken: token);
        return await _productRepository.CreateAsync(product, token);
    }

    public Task<Product?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        return _productRepository.GetByIdAsync(id, token);
    }

    public Task<IEnumerable<Product>> GetAllAsync(CancellationToken token = default)
    {
        return _productRepository.GetAllAsync(token);
    }

    public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
    {
        return _productRepository.DeleteByIdAsync(id, token); 
    }

    public async Task<Product?> UpdateAsync(Product product, CancellationToken token = default)
    {
        await _productValidator.ValidateAndThrowAsync(product, cancellationToken: token);
        var productExists = await _productRepository.ExistsByIdAsync(product.Id, token);
        if (!productExists)
        {
            return null;
        }

        await _productRepository.UpdateAsync(product, token);
        return product;
    }
}