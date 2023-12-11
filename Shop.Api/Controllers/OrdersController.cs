using System.Security.Claims;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Api.Contracts.Requests;
using Shop.Api.Mapping;
using Shop.Api.Models;
using Shop.Api.Services;

namespace Shop.Api.Controllers;


[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly ICurrentUserService _currentUserService;

    public OrdersController(IOrderService orderService, ICurrentUserService currentUserService)
    {
        _orderService = orderService;
        _currentUserService = currentUserService;
    }

    [Authorize]
    [HttpPost(ApiEndpoints.Order.Create)]
    public async Task<ActionResult<Order>> CreateOrder([FromBody] CreateOrderRequest request, CancellationToken token)
    {
        try
        {
            // Получаем идентификатор текущего пользователя
            var currentUser = _currentUserService.GetCurrentUser();
            
            // Передаем идентификатор пользователя в CreateAsync
            var order = await _orderService.CreateAsync(request, currentUser.Id, token);
            var orderResponse = order.MapToResponse();
            return Ok(orderResponse);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }

        return BadRequest("Something went wrong");
    }

    [HttpGet(ApiEndpoints.Order.GetAll)]
    public async Task<IActionResult> GetAll(CancellationToken token)
    {
        var orders = await _orderService.GetAllAsync(token);
        var response = orders.MapToResponse();
        return Ok(response);
    }
}