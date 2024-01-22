using Shop.Contracts.Requests;
using Shop.Domain.Orders;
using Shop.Domain.Products;

namespace Shop.Application.Services;

public interface IOrderService
{
    Task<Order> CreateAsync(CreateOrderRequest orderRequest, Guid userId, CancellationToken token = default);

    Task<IEnumerable<Order>> GetAllAsync(CancellationToken token = default);
}