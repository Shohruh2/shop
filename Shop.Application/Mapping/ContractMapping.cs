using Shop.Contracts.Requests;
using Shop.Contracts.Requests.CustomerRequests;
using Shop.Contracts.Requests.ProductRequests;
using Shop.Contracts.Responses;
using Shop.Contracts.Responses.CustomerResponses;
using Shop.Contracts.Responses.OrderResponses;
using Shop.Contracts.Responses.ProductResponses;
using Shop.Domain.Customers;
using Shop.Domain.Orders;
using Shop.Domain.Products;

namespace Shop.Application.Mapping;

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
        customer.Gender = request.Gender;
        customer.Birthday = request.Birthday;
    }
    
    public static CustomerResponse MapToResponse(this Customer customer)
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
    
    
    public static OrderResponse MapToResponse(this Order order)
    {
        return new OrderResponse
        {
            Id = order.Id,
            DateTime = order.DateTime,
            OrderItems = order.OrderItems.Select(x => new OrderResponseItemDto
            {
                Id = x.Id,
                ProductId = x.ProductId,
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice
            }).ToList(),
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