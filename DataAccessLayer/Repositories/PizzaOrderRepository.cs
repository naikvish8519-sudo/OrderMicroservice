using eCommerce.OrdersMicroservice.DataAccessLayer.Context;
using eCommerce.OrdersMicroservice.DataAccessLayer.Entities;
using eCommerce.OrdersMicroservice.DataAccessLayer.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace eCommerce.OrdersMicroservice.DataAccessLayer.Repositories;

public class PizzaOrderRepository : IPizzaOrderRepository
{
    private readonly ApplicationDbContext _dbContext;

    public PizzaOrderRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PizzaOrder?> AddOrderAsync(PizzaOrder order)
    {
        order.OrderID = Guid.NewGuid();
        order.OrderDate = DateTime.UtcNow;
        _dbContext.PizzaOrders.Add(order);
        await _dbContext.SaveChangesAsync();
        return order;
    }

    public async Task<IEnumerable<PizzaOrder>> GetAllOrdersAsync()
    {
        return await _dbContext.PizzaOrders.ToListAsync();
    }

    public async Task<IEnumerable<PizzaOrder>> GetOrdersByUserAsync(Guid userId, bool onlyOrdered = true)
    {
        return await _dbContext.PizzaOrders
            .Where(o => o.UserID == userId && o.IsOrdered == onlyOrdered)
            .ToListAsync();
    }

    public async Task<PizzaOrder?> GetOrderByIdAsync(Guid orderId)
    {
        return await _dbContext.PizzaOrders.FirstOrDefaultAsync(o => o.OrderID == orderId);
    }

    public async Task<bool> DeleteOrderAsync(Guid orderId)
    {
        var order = await _dbContext.PizzaOrders.FirstOrDefaultAsync(o => o.OrderID == orderId);
        if (order == null)
            return false;

        _dbContext.PizzaOrders.Remove(order);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<PizzaOrder?> UpdateOrderAsync(PizzaOrder updatedOrder)
    {
        var existing = await _dbContext.PizzaOrders.FirstOrDefaultAsync(o => o.OrderID == updatedOrder.OrderID);
        if (existing == null)
            return null;

        existing.PizzaSize = updatedOrder.PizzaSize;
        existing.Toppings = updatedOrder.Toppings;
        existing.UnitPrice = updatedOrder.UnitPrice;
        existing.Quantity = updatedOrder.Quantity;
        existing.IsOrdered = updatedOrder.IsOrdered;

        await _dbContext.SaveChangesAsync();
        return existing;
    }

    public async Task<PizzaOrder?> GetPizzaOrderByCondition(Expression<Func<PizzaOrder, bool>> predicate)
    {
        return await _dbContext.PizzaOrders.FirstOrDefaultAsync(predicate);
    }

    public async Task<List<PizzaOrder>> GetPizzaOrdersByCondition(Expression<Func<PizzaOrder, bool>> predicate)
    {
        return await _dbContext.PizzaOrders.Where(predicate).ToListAsync();
    }

}
