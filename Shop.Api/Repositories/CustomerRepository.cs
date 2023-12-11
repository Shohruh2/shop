using Microsoft.EntityFrameworkCore;
using Shop.Api.Contracts.Requests;
using Shop.Api.Database;
using Shop.Api.Models;

namespace Shop.Api.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly ApiDbContext _dbContext;

    public CustomerRepository(ApiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> CreateAsync(Customer? customer, CancellationToken token = default)
    {
        _dbContext.Customers.Add(customer);
        var rowsAffected = await _dbContext.SaveChangesAsync(token);
        return rowsAffected > 0;
    }

    public async Task<bool> UpdateAsync(CancellationToken token = default)
    {
        var rowsAffected = await _dbContext.SaveChangesAsync(token);
        return rowsAffected > 0;
    }

    public async Task<Customer?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        var customer = await _dbContext.Customers.FindAsync(id, token);
        return customer;
    }

    public async Task<IEnumerable<Customer?>> GetAllAsync(CancellationToken token = default)
    {
        var customers = await _dbContext.Customers.ToListAsync(token);
        return customers;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken token = default)
    {
        var customer = await _dbContext.Customers.FindAsync(id, token);
        if (customer == null)
        {
            return false;
        }

        _dbContext.Customers.Remove(customer);
        var rowsAffected = await _dbContext.SaveChangesAsync(token);
        return rowsAffected > 0;
    }
    
    // public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default)
    // {
    //     var count = await _dbContext.Customers.CountAsync(x => x.Id == id, token);
    //     return count > 0;
    // }
}