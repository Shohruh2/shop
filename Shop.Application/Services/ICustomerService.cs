using Shop.Contracts.Requests;
using Shop.Domain.Customers;

namespace Shop.Application.Services;

public interface ICustomerService
{
    Task<Customer> CreateAsync(CreateCustomerRequest createCustomerRequest, CancellationToken token = default);

    Task<bool> DeleteAsync(Guid id, CancellationToken token = default);

    Task<Customer?> GetByIdAsync(Guid id, CancellationToken token = default);

    Task<IEnumerable<Customer>> GetAllAsync(CancellationToken token = default);

    Task<Customer?> UpdateAsync(Guid id, UpdateCustomerRequest customerUpdate, CancellationToken token = default);
}