using Shop.Application.Mapping;
using Shop.Contracts.Requests;
using Shop.Domain.Orders;
using Shop.Domain.Products;

namespace Shop.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;

    public OrderService(IOrderRepository orderRepository, IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
    }


    public async Task<Order?> CreateAsync(CreateOrderRequest orderRequest, Guid userId, CancellationToken token = default)
    {
        var order = await orderRequest.MapToOrderAsync(_productRepository);

        var totalPrice = order.OrderItems.Sum(item => item.TotalPrice);

        var customer = await _orderRepository.GetCustomerAsync(userId, token);
        if (customer == null || customer.Balance < totalPrice)
        {
            return null;
        }

        customer.Balance -= totalPrice;
        await _orderRepository.UpdateCustomerAsync(customer, token);

        await _orderRepository.AddOrderAsync(order, token);

        return order;
    }

    public async Task<IEnumerable<Order>> GetAllAsync(CancellationToken token = default)
    {        
        return await _orderRepository.GetAllAsync(token);
    }
}
