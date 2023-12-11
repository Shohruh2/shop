using Shop.Api.Contracts.Requests;
using Shop.Api.Models;

namespace Shop.Api.Repositories;

public interface IOrderRepository
{
    Task<Customer?> GetCustomerAsync(Guid id, CancellationToken token = default);

    Task<bool> UpdateCustomerAsync(Customer customer, CancellationToken token = default);

    Task<bool> AddOrderAsync(Order order, CancellationToken token=default);


    Task<IEnumerable<Order>> GetAllAsync(CancellationToken token = default);
}
