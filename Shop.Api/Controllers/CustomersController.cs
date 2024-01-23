using System.Net;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Mapping;
using Shop.Application.Services;
using Shop.Contracts.Requests;
using Shop.Contracts.Requests.CustomerRequests;
using Shop.Contracts.Responses;
using Shop.Contracts.Responses.CustomerResponses;
using Shop.Contracts.Responses.StandartResponse;
using ResponseError = Shop.Contracts.Responses.StandartResponse.ResponseError;

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
    public async Task<ActionResult<Response<CustomersResponse>>> GetAll(CancellationToken token)
    {
        var customers = await _customerService.GetAllAsync(token);
        var customerResponse = customers.MapToResponse();
        var response = Response<CustomersResponse>.CreateSuccessResponse(customerResponse);
        return Ok(response);
    }

    [HttpGet(ApiEndpoints.Customer.Get)]
    public async Task<ActionResult<Response<CustomerResponse>>> Get([FromRoute] Guid id,
        CancellationToken token)
    {
        var customer = await _customerService.GetByIdAsync(id, token);
        if (customer == null)
        {
            var notFoundResponse = Response<CustomerResponse>.CreateErrorResponse(new ResponseError
            {
                Message = "Not found",
                Code = HttpStatusCode.NotFound.ToString()
            });
            return NotFound(notFoundResponse);
        }

        var customerResponse = customer.MapToResponse();
        var response = Response<CustomerResponse>.CreateSuccessResponse(customerResponse);
        return Ok(response);
    }

    [HttpPost(ApiEndpoints.Customer.Create)]
    public async Task<ActionResult<Response<CustomerResponse>>> Create([FromBody] CreateCustomerRequest customerRequest,
        CancellationToken token)
    {
        var customer = await _customerService.CreateAsync(customerRequest, token);
        var customerResponse = customer.MapToResponse();
        var response = Response<CustomerResponse>.CreateSuccessResponse(customerResponse);
        return Ok(response);
    }

    [HttpPut(ApiEndpoints.Customer.Update)]
    public async Task<ActionResult<Response<CustomerResponse>>> Update([FromRoute] Guid id,
        [FromBody] UpdateCustomerRequest updateCustomerRequest, CancellationToken token)
    {
        var customerToUpdate = await _customerService.UpdateAsync(id, updateCustomerRequest, token);
        var customerResponse = customerToUpdate!.MapToResponse();
        var response = Response<CustomerResponse>.CreateSuccessResponse(customerResponse);
        return Ok(response);
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