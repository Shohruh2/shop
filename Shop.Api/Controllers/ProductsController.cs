using Microsoft.AspNetCore.Mvc;
using Shop.Application.Mapping;
using Shop.Application.Services;
using Shop.Contracts.Requests;

namespace Shop.Api.Controllers;

[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet(ApiEndpoints.Product.Get)]
    public async Task<IActionResult> Get([FromRoute] Guid id,
        CancellationToken token)
    {
        var product = await _productService.GetByIdAsync(id, token);
        if (product == null)
        {
            return NotFound();
        }

        var productResponse = product.MapToResponse();
        return Ok(productResponse);
    }


    [HttpGet(ApiEndpoints.Product.GetAll)]
    public async Task<IActionResult> GetAll(CancellationToken token)
    {
        var products = await _productService.GetAllAsync(token);

        var productsResponse = products.MapToResponse();
        return Ok(productsResponse);
    }

    [HttpPost(ApiEndpoints.Product.Create)]
    public async Task<IActionResult> Create([FromBody] CreateProductRequest request,
        CancellationToken token)
    {
        var product = request.MapToProduct();
        await _productService.CreateAsync(product, token);
        var productResponse = product.MapToResponse();
        return Ok(productResponse);
    }

    [HttpPut(ApiEndpoints.Product.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid id,
        [FromBody] UpdateProductRequest request,
        CancellationToken token)
    {
        var product = request.MapToProduct(id);
        var updatedProduct = await _productService.UpdateAsync(product, token);
        if (updatedProduct == null)
        {
            return NotFound();
        }

        var response = updatedProduct.MapToResponse();
        return Ok(response);
    }

    [HttpDelete(ApiEndpoints.Product.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken token)
    {
        var deleted= await _productService.DeleteByIdAsync(id, token);
        if (!deleted)
        {
            return NotFound();
        }

        return Ok();
    }
}