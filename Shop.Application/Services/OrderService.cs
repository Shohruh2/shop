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


    public async Task<Order> CreateAsync(CreateOrderRequest orderRequest, Guid userId,
        CancellationToken token = default)
    {
        var products = new List<Product>();
        var order = new Order
        {
            Id = Guid.NewGuid(),
            DateTime = DateTime.Now
        };
        decimal grandTotal = 0;
        int quantity = 0;
        foreach (var item in orderRequest.Items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId, token);
            if (product == null)
            {
                throw new Exception("Product not found");
            }
            
            products.Add(product);
            if (product.Quantity < item.Quantity)
            {
                throw new Exception($"We don't have so much product: {product.Name} quantity {product.Quantity}");
            }

            quantity += item.Quantity;
            
            var orderItem = new OrderItem
            {
                Id = Guid.NewGuid(),
                Quantity = item.Quantity,
                ProductId = product.Id,
                UnitPrice = product.Price
            };
            order.OrderItems.Add(orderItem);
            grandTotal += orderItem.TotalPrice;
        }

        var customer = await _orderRepository.GetCustomerAsync(userId, token);
        if (customer == null)
        {
            throw new Exception($"Customer not found");
        }

        if (customer.Balance < grandTotal)
        {
            throw new Exception("You don't have so much money bro((");
        }

        customer.Balance -= grandTotal;
        await _orderRepository.UpdateCustomerAsync(customer, token);

        foreach (var product in products)
        {
            product.Quantity -= quantity;
            await _productRepository.UpdateAsync(product,token);
        }
        
        await _orderRepository.AddOrderAsync(order, token);
            
        return order;
    }
    public async Task<IEnumerable<Order>> GetAllAsync(CancellationToken token = default)
    {        
        return await _orderRepository.GetAllAsync(token);
    }
}
