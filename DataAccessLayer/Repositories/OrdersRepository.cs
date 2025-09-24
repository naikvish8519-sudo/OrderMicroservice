//using eCommerce.OrdersMicroservice.DataAccessLayer.Context;
//using eCommerce.OrdersMicroservice.DataAccessLayer.Entities;
//using eCommerce.OrdersMicroservice.DataAccessLayer.RepositoryContracts;
//using Microsoft.EntityFrameworkCore;
//using System.Linq.Expressions;

//namespace eCommerce.OrdersMicroservice.DataAccessLayer.Repositories;

//public class OrdersRepository : IOrdersRepository
//{
//    private readonly ApplicationDbContext _dbContext;

//    public OrdersRepository(ApplicationDbContext dbContext)
//    {
//        _dbContext = dbContext;
//    }

//    public async Task<Order?> AddOrder(Order order)
//    {
//        order.OrderID = Guid.NewGuid();

//        foreach (var item in order.OrderItems)
//        {
//            item.OrderItemID = Guid.NewGuid();
//            item.OrderID = order.OrderID;
//        }

//        _dbContext.Orders.Add(order);
//        await _dbContext.SaveChangesAsync();
//        return order;
//    }

//    public async Task<bool> DeleteOrder(Guid orderID)
//    {
//        var existingOrder = await _dbContext.Orders
//            .Include(o => o.OrderItems)
//            .FirstOrDefaultAsync(o => o.OrderID == orderID);

//        if (existingOrder == null)
//            return false;

//        _dbContext.Orders.Remove(existingOrder);
//        await _dbContext.SaveChangesAsync();
//        return true;
//    }

//    public async Task<IEnumerable<Order>> GetOrders()
//    {
//        return await _dbContext.Orders
//            .Include(o => o.OrderItems)
//            .ToListAsync();
//    }

//    public async Task<IEnumerable<Order>> GetOrdersByCondition(Expression<Func<Order, bool>> condition)
//    {
//        return await _dbContext.Orders
//            .Include(o => o.OrderItems)
//            .Where(condition)
//            .ToListAsync();
//    }

//    public async Task<Order?> GetOrderByCondition(Expression<Func<Order, bool>> condition)
//    {
//        return await _dbContext.Orders
//            .Include(o => o.OrderItems)
//            .FirstOrDefaultAsync(condition);
//    }

//    public async Task<Order?> UpdateOrder(Order order)
//    {
//        var existingOrder = await _dbContext.Orders
//            .Include(o => o.OrderItems)
//            .FirstOrDefaultAsync(o => o.OrderID == order.OrderID);

//        if (existingOrder == null)
//            return null;

//        // Update order fields
//        existingOrder.OrderDate = order.OrderDate;
//        existingOrder.TotalBill = order.TotalBill;

//        // Remove items that no longer exist
//        foreach (var existingItem in existingOrder.OrderItems.ToList())
//        {
//            if (!order.OrderItems.Any(i => i.OrderItemID == existingItem.OrderItemID))
//            {
//                _dbContext.OrderItems.Remove(existingItem);
//            }
//        }

//        // Add or update items
//        foreach (var incomingItem in order.OrderItems)
//        {
//            var matchingExistingItem = existingOrder.OrderItems
//                .FirstOrDefault(i => i.OrderItemID == incomingItem.OrderItemID);

//            if (matchingExistingItem != null)
//            {
//                matchingExistingItem.ProductID = incomingItem.ProductID;
//                matchingExistingItem.Quantity = incomingItem.Quantity;
//                matchingExistingItem.UnitPrice = incomingItem.UnitPrice;
//                matchingExistingItem.TotalPrice = incomingItem.TotalPrice;
//            }
//            else
//            {
//                if (incomingItem.OrderItemID == Guid.Empty)
//                {
//                    incomingItem.OrderItemID = Guid.NewGuid();
//                }

//                incomingItem.OrderID = existingOrder.OrderID;
//                existingOrder.OrderItems.Add(incomingItem);
//            }
//        }

//        await _dbContext.SaveChangesAsync();
//        return existingOrder;
//    }
//}
