using Microsoft.AspNetCore.Mvc;
using Shop.Application.Mapping;
using Shop.Application.Services;
using Shop.Contracts.Requests;

namespace Shop.Api.Controllers;

[ApiController]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet(ApiEndpoints.Customer.GetAll)]
    public async Task<IActionResult> GetAll(CancellationToken token)
    {
        var customers = await _customerService.GetAllAsync(token);
        var response = customers.MapToResponse();
        return Ok(response);
    }

    [HttpGet(ApiEndpoints.Customer.Get)]
    public async Task<IActionResult> Get([FromRoute] Guid id,
        CancellationToken token)
    {
        var customer = await _customerService.GetByIdAsync(id, token);
        if (customer == null)
        {
            return NotFound();
        }

        var customerResponse = customer.MapToResponse();
        return Ok(customerResponse);
    }

    [HttpPost(ApiEndpoints.Customer.Create)]
    public async Task<IActionResult> Create([FromBody] CreateCustomerRequest customerRequest,
        CancellationToken token)
    {
        var customer = await _customerService.CreateAsync(customerRequest, token);
        var response = customer.MapToResponse();
        return Ok(response);
    }

    [HttpPut(ApiEndpoints.Customer.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid id,
        [FromBody] UpdateCustomerRequest updateCustomerRequest, CancellationToken token)
    {
        var customerToUpdate = await _customerService.UpdateAsync(id, updateCustomerRequest, token);
        if (customerToUpdate != null)
        {
            var response = customerToUpdate.MapToResponse();
            return Ok(response);
        }

        return NotFound();
    }

    [HttpDelete(ApiEndpoints.Customer.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id,
        CancellationToken token)
    {
        var customerToDelete = await _customerService.DeleteAsync(id, token);
        if (!customerToDelete)
        {
            return NotFound();
        }

        return Ok();
    }
}