﻿using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.Mapping;
using Shop.Application.Services;
using Shop.Contracts.Requests;
using Shop.Contracts.Responses;
using Shop.Domain.Orders;

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
        try
        {
            // Получаем идентификатор текущего пользователя
            var currentUser = _currentUserService.GetCurrentUser();
            
            // Передаем идентификатор пользователя в CreateAsync
            var order = await _orderService.CreateAsync(request, currentUser.Id, token);
            var orderResponse = order.MapToResponse();
            var response = Response<OrderResponse?>.CreateSuccessResponse(orderResponse);
            return Ok(response);
        }
        catch (Exception ex)
        {
            var response = Response<OrderResponse?>.CreateFailedResponse(new ResponseError
            {
                Message = ex.Message,
                Code = HttpStatusCode.BadRequest.ToString()
            });
            return StatusCode(400, response);
        }
    }

    [HttpGet(ApiEndpoints.Order.GetAll)]
    public async Task<IActionResult> GetAll(CancellationToken token)
    {
        var orders = await _orderService.GetAllAsync(token);
        var response = orders.MapToResponse();
        return Ok(response);
    }
}