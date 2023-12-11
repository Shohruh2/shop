using Shop.Api.Contracts.Requests;
using Shop.Api.Contracts.Responses;
using Shop.Api.Models;
using Shop.Api.Repositories;

namespace Shop.Api.Mapping;

public static class ContractMapping
{
    public static Product MapToProduct(this CreateProductRequest request)
    {
        return new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Quantity = request.Quantity
        };
    }

    public static Product MapToProduct(this UpdateProductRequest request, Guid id)
    {
        return new Product
        {
            Id = id,
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Quantity = request.Quantity
        };
    }

    public static ProductResponse MapToResponse(this Product product)
    {
        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Quantity = product.Quantity
        };
    }
    
    public static ProductsResponse MapToResponse(this IEnumerable<Product> products)
    {
        return new ProductsResponse
        {
            Items = products.Select(MapToResponse)
        };
    }
    
    
    public static Customer? MapToCustomer(this CreateCustomerRequest request)
    {
        return new Customer
        {
            Id = request.Id,
            Name = request.Name,
            Surname = request.Surname,
            Gender = request.Gender,
            Birthday = request.Birthday,
            Balance = 0
        };
    }

    public static void MapToCustomer(this UpdateCustomerRequest request, Customer customer)
    {
        customer.Name = request.Name;
        customer.Surname = request.Surname;
        customer.Birthday = request.Birthday;
    }
    
    public static CustomerResponse MapToResponse(this Customer? customer)
    {
        return new CustomerResponse
        {
            Id = customer.Id,
            Name = customer.Name,
            Surname = customer.Surname,
            Gender = customer.Gender,
            Birthday = customer.Birthday,
            Balance = customer.Balance
        };
    }
    
    public static CustomersResponse MapToResponse(this IEnumerable<Customer?> customers)
    {
        return new CustomersResponse
        {
            Items = customers.Select(MapToResponse)
        };
    }

    public static async Task<Order> MapToOrderAsync(this CreateOrderRequest request, IProductRepository productRepository)
    {
        var orderItemsTasks = request.Items.Select(async item =>
        {
            var unitPrice = await GetProductPrice(item.ProductId, productRepository);

            var product = await productRepository.GetByIdAsync(item.ProductId);

            if (product == null || product.Quantity < item.Quantity)
            {
                throw new InvalidOperationException("Not enough quantity in stock for product with ID " + item.ProductId);
            }

            product.Quantity -= item.Quantity;
            await productRepository.UpdateAsync(product);
            
            return new OrderItem
            {
                Id = Guid.NewGuid(),
                ProductId = item.ProductId,
                UnitPrice = unitPrice,
                Quantity = item.Quantity
            };
        });

        var orderItems = await Task.WhenAll(orderItemsTasks);

        var order = new Order
        {
            Id = Guid.NewGuid(),
            DateTime = DateTime.Now,
            OrderItems = orderItems.ToList(),
        };

        return order;
    }
    private static async Task<decimal> GetProductPrice(Guid itemProductId, IProductRepository productRepository)
    {
        var product = await productRepository.GetByIdAsync(itemProductId);

        if (product != null)
        {
            return product.Price;
        }

        throw new InvalidOperationException($"Product with Id {itemProductId} not found.");
    }
    
    public static OrderResponse MapToResponse(this Order? order)
    {
        return new OrderResponse
        {
            Id = order.Id,
            DateTime = order.DateTime,
            OrderItems = order.OrderItems,
        };
    }
    
    public static OrdersResponse MapToResponse(this IEnumerable<Order?> orders)
    {
        return new OrdersResponse
        {
            Items = orders.Select(MapToResponse)
        };
    }
}