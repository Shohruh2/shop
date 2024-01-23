using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Mapping;
using Shop.Application.Services;
using Shop.Contracts.Requests;
using Shop.Contracts.Requests.OrderRequests;
using Shop.Contracts.Responses;
using Shop.Contracts.Responses.OrderResponses;
using Shop.Contracts.Responses.StandartResponse;

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
    public async Task<ActionResult<Response<OrderResponse>>> CreateOrder([FromBody] CreateOrderRequest request, CancellationToken token)
    {
        // Получаем идентификатор текущего пользователя
        var currentUser = _currentUserService.GetCurrentUser();

        // Передаем идентификатор пользователя в CreateAsync
        var order = await _orderService.CreateAsync(request, currentUser.Id, token);
        var orderResponse = order.MapToResponse();
        var response = Response<OrderResponse?>.CreateSuccessResponse(orderResponse);
        return Ok(response);
    }

    [HttpGet(ApiEndpoints.Order.GetAll)]
    public async Task<ActionResult<Response<OrdersResponse>>> GetAll(CancellationToken token)
    {
        var orders = await _orderService.GetAllAsync(token);
        var orderResponse = orders.MapToResponse();
        var response = Response<OrdersResponse>.CreateSuccessResponse(orderResponse);
        return Ok(response);
    }
}