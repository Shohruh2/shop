using Shop.Api.Contracts.Requests;
using Shop.Api.Models;

namespace Shop.Api.Services;

public interface IOrderService
{
    Task<Order?> CreateAsync(CreateOrderRequest orderRequest, Guid userId, CancellationToken token = default);

    Task<IEnumerable<Order>> GetAllAsync(CancellationToken token = default);
}