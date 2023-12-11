using Microsoft.EntityFrameworkCore;
using Shop.Api.Database;
using Shop.Api.Models;

namespace Shop.Api.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly ApiDbContext _dbContext;

    public OrderRepository(ApiDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<Customer?> GetCustomerAsync(Guid id, CancellationToken token = default)
    {
        var customer = await _dbContext.Customers.FindAsync(id);
        if (customer == null)
        {
            return null;
        }

        return customer;
    }

    public async Task<bool> UpdateCustomerAsync(Customer customer, CancellationToken token = default)
    {
        var existingCustomer = await _dbContext.Customers.FindAsync(customer.Id);

        if (existingCustomer != null)
        {
            existingCustomer.Balance = customer.Balance;

            await _dbContext.SaveChangesAsync(token);
            return true;
        }

        return false;
    }

    public async Task<bool> AddOrderAsync(Order order, CancellationToken token=default)
    {
        await _dbContext.Orders.AddAsync(order);
        var rowsAffected = await _dbContext.SaveChangesAsync(token);
        return rowsAffected > 0;
    }

    public async Task<IEnumerable<Order>> GetAllAsync(CancellationToken token = default)
    {
        return await _dbContext.Orders.Include(o=> o.OrderItems).ToListAsync(token);
    }
}