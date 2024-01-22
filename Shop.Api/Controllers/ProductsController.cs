using System.Net;
using Microsoft.AspNetCore.Mvc;
using Shop.Api.Migrations;
using Shop.Application.Mapping;
using Shop.Application.Services;
using Shop.Contracts.Requests;
using Shop.Contracts.Responses;

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
    public async Task<ActionResult<Response<ProductResponse>>> Get([FromRoute] Guid id,
        CancellationToken token)
    {
        var product = await _productService.GetByIdAsync(id, token);
        if (product == null)
        {
            var responseFail = Response<ProductResponse?>.CreateErrorResponse(new ResponseError
            {
                Message = "Product not found",
                Code = HttpStatusCode.NotFound.ToString()
            });
            return StatusCode(404, responseFail);
        }
        
        var productResponse = product.MapToResponse();
        var response = Response<ProductResponse?>.CreateSuccessResponse(productResponse);
        return Ok(response);
    }


    [HttpGet(ApiEndpoints.Product.GetAll)]
    public async Task<ActionResult<Response<ProductsResponse>>> GetAll(CancellationToken token)
    {
        var products = await _productService.GetAllAsync(token);
        var productsResponse = products.MapToResponse();
        var response = Response<ProductsResponse?>.CreateSuccessResponse(productsResponse); 
        return Ok(response);
    }

    [HttpPost(ApiEndpoints.Product.Create)]
    public async Task<ActionResult<Response<ProductResponse>>> Create([FromBody] CreateProductRequest request,
        CancellationToken token)
    {
        var product = request.MapToProduct();
        await _productService.CreateAsync(product, token);
        var productResponse = product.MapToResponse();
        var response = Response<ProductResponse?>.CreateSuccessResponse(productResponse);
        return Ok(response);
    }

    [HttpPut(ApiEndpoints.Product.Update)]
    public async Task<ActionResult<Response<ProductResponse>>> Update([FromRoute] Guid id,
        [FromBody] UpdateProductRequest request,
        CancellationToken token)
    {
        var product = request.MapToProduct(id);
        var updatedProduct = await _productService.UpdateAsync(product, token);
        if (updatedProduct == null)
        {
            return NotFound();
        }

        var productResponse = updatedProduct.MapToResponse();
        var response = Response<ProductResponse?>.CreateSuccessResponse(productResponse);
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