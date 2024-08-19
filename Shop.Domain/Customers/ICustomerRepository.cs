namespace Shop.Domain.Customers;

public interface ICustomerRepository
{
    Task<bool> CreateAsync(Customer customer, CancellationToken token = default);

    Task<bool> UpdateAsync(CancellationToken token = default);

    Task<Customer?> GetByIdAsync(Guid id, CancellationToken token = default);

    Task<IEnumerable<Customer>> GetAllAsync(CancellationToken token = default);

    Task<bool> DeleteAsync(Guid id, CancellationToken token = default);
    
    // Task<bool> ExistsByIdAsync(Guid id, CancellationToken token);
}