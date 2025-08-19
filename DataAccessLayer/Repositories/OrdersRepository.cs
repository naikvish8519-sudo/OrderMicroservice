//using eCommerce.OrdersMicroservice.DataAccessLayer.Entities;
//using eCommerce.OrdersMicroservice.DataAccessLayer.RepositoryContracts;
//using MongoDB.Driver;


//namespace eCommerce.OrdersMicroservice.DataAccessLayer.Repositories;


//public class OrdersRepository : IOrdersRepository
//{
//  private readonly IMongoCollection<Order> _orders;
//  private readonly string collectionName = "orders";

//  public OrdersRepository(IMongoDatabase mongoDatabase)
//  {
//    _orders = mongoDatabase.GetCollection<Order>(collectionName);
//  }


//  public async Task<Order?> AddOrder(Order order)
//  {
//    order.OrderID = Guid.NewGuid();
//    order._id = order.OrderID;

//    foreach (OrderItem orderItem in order.OrderItems)
//    {
//      orderItem._id = Guid.NewGuid();
//    }

//    await _orders.InsertOneAsync(order);
//    return order;
//  }


//  public async Task<bool> DeleteOrder(Guid orderID)
//  {
//    FilterDefinition<Order> filter = Builders<Order>.Filter.Eq(temp => temp.OrderID, orderID);

//    Order? existingOrder = (await _orders.FindAsync(filter)).FirstOrDefault();

//    if (existingOrder == null) {
//      return false;
//    }

//    DeleteResult deleteResult = await _orders.DeleteOneAsync(filter);

//    return deleteResult.DeletedCount > 0;
//  }


//  public async Task<Order?> GetOrderByCondition(FilterDefinition<Order> filter)
//  {
//    return (await _orders.FindAsync(filter)).FirstOrDefault();
//  }


//  public async Task<IEnumerable<Order>> GetOrders()
//  {
//    return (await _orders.FindAsync(Builders<Order>.Filter.Empty)).ToList();
//  }


//  public async Task<IEnumerable<Order?>> GetOrdersByCondition(FilterDefinition<Order> filter)
//  {
//    return (await _orders.FindAsync(filter)).ToList();
//  }


//  public async Task<Order?> UpdateOrder(Order order)
//  {
//    FilterDefinition<Order> filter = Builders<Order>.Filter.Eq(temp => temp.OrderID, order.OrderID);

//    Order? existingOrder = (await _orders.FindAsync(filter)).FirstOrDefault();

//    if (existingOrder == null)
//    {
//      return null;
//    }
//    order._id = existingOrder._id;

//    ReplaceOneResult replaceOneResult = await _orders.ReplaceOneAsync(filter, order);

//    return order;
//  }
//}
using eCommerce.OrdersMicroservice.DataAccessLayer.Context;
using eCommerce.OrdersMicroservice.DataAccessLayer.Entities;
using eCommerce.OrdersMicroservice.DataAccessLayer.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace eCommerce.OrdersMicroservice.DataAccessLayer.Repositories;

public class OrdersRepository : IOrdersRepository
{
    private readonly ApplicationDbContext _dbContext;

    public OrdersRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Order?> AddOrder(Order order)
    {
        order.OrderID = Guid.NewGuid();

        foreach (var item in order.OrderItems)
        {
            item.OrderItemID = Guid.NewGuid(); // Assuming you replaced `_id` with `OrderItemID`
            item.OrderID = order.OrderID;
        }

        _dbContext.Orders.Add(order);
        await _dbContext.SaveChangesAsync();
        return order;
    }

    public async Task<bool> DeleteOrder(Guid orderID)
    {
        var existingOrder = await _dbContext.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.OrderID == orderID);

        if (existingOrder == null)
            return false;

        _dbContext.Orders.Remove(existingOrder);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Order>> GetOrders()
    {
        return await _dbContext.Orders
            .Include(o => o.OrderItems)
            .ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetOrdersByCondition(Expression<Func<Order, bool>> condition)
    {
        return await _dbContext.Orders
            .Include(o => o.OrderItems)
            .Where(condition)
            .ToListAsync();
    }

    public async Task<Order?> GetOrderByCondition(Expression<Func<Order, bool>> condition)
    {
        return await _dbContext.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(condition);
    }

    //public async Task<Order?> UpdateOrder(Order order)
    //{
    //    var existingOrder = await _dbContext.Orders
    //        .Include(o => o.OrderItems)
    //        .FirstOrDefaultAsync(o => o.OrderID == order.OrderID);

    //    if (existingOrder == null)
    //        return null;

    //    // Update order fields
    //    existingOrder.OrderDate = order.OrderDate;
    //    existingOrder.TotalBill = order.TotalBill;

    //    // Replace order items
    //    _dbContext.OrderItems.RemoveRange(existingOrder.OrderItems);
    //    foreach (var item in order.OrderItems)
    //    {
    //        item.OrderItemID = Guid.NewGuid(); // New ID
    //        item.OrderID = order.OrderID;
    //    }

    //    existingOrder.OrderItems = order.OrderItems;

    //    await _dbContext.SaveChangesAsync();
    //    return existingOrder;
    //}

    //public async Task<Order?> UpdateOrder(Order order)
    //{
    //    var existingOrder = await _dbContext.Orders
    //        .Include(o => o.OrderItems)
    //        .FirstOrDefaultAsync(o => o.OrderID == order.OrderID);

    //    if (existingOrder == null)
    //        return null;

    //    // Update order fields
    //    existingOrder.OrderDate = order.OrderDate;
    //    existingOrder.TotalBill = order.TotalBill;

    //    // Step 1: Delete removed items
    //    foreach (var existingItem in existingOrder.OrderItems.ToList())
    //    {
    //        if (!order.OrderItems.Any(i => i.OrderItemID == existingItem.OrderItemID))
    //        {
    //            _dbContext.OrderItems.Remove(existingItem);
    //        }
    //    }

    //    // Step 2: Add/update items
    //    foreach (var item in order.OrderItems)
    //    {
    //        var existingItem = existingOrder.OrderItems
    //            .FirstOrDefault(i => i.OrderItemID == item.OrderItemID);

    //        if (existingItem != null)
    //        {
    //            // Update existing item
    //            existingItem.ProductID = item.ProductID;
    //            existingItem.Quantity = item.Quantity;
    //            existingItem.UnitPrice = item.UnitPrice;
    //            existingItem.TotalPrice = item.TotalPrice;
    //        }
    //        else
    //        {
    //            // Add new item
    //            item.OrderItemID = Guid.NewGuid();
    //            item.OrderID = order.OrderID;
    //            existingOrder.OrderItems.Add(item);
    //        }
    //    }

    //    await _dbContext.SaveChangesAsync();
    //    return existingOrder;
    //}
    public async Task<Order?> UpdateOrder(Order order)
    {
        // Step 0: Load the existing order including its items
        var existingOrder = await _dbContext.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.OrderID == order.OrderID);

        if (existingOrder == null)
            return null;

        // Step 1: Update top-level order fields
        existingOrder.OrderDate = order.OrderDate;
        existingOrder.TotalBill = order.TotalBill;

        // Step 2: Remove order items that were deleted by the user
        foreach (var existingItem in existingOrder.OrderItems.ToList()) // ToList avoids modifying during iteration
        {
            bool isStillPresent = order.OrderItems.Any(i => i.OrderItemID == existingItem.OrderItemID);
            if (!isStillPresent)
            {
                _dbContext.OrderItems.Remove(existingItem);
            }
        }

        // Step 3: Add new items or update existing ones
        foreach (var incomingItem in order.OrderItems)
        {
            var matchingExistingItem = existingOrder.OrderItems
                .FirstOrDefault(i => i.OrderItemID == incomingItem.OrderItemID);

            if (matchingExistingItem != null)
            {
                // Update existing item
                matchingExistingItem.ProductID = incomingItem.ProductID;
                matchingExistingItem.Quantity = incomingItem.Quantity;
                matchingExistingItem.UnitPrice = incomingItem.UnitPrice;
                matchingExistingItem.TotalPrice = incomingItem.TotalPrice;
            }
            else
            {
                // Add new item — generate new ID and set foreign key
                if (incomingItem.OrderItemID == Guid.Empty)
                {
                    incomingItem.OrderItemID = Guid.NewGuid();
                }

                incomingItem.OrderID = existingOrder.OrderID;
                existingOrder.OrderItems.Add(incomingItem);
                incomingItem.OrderID = existingOrder.OrderID;

                existingOrder.OrderItems.Add(incomingItem);
            }
        }

        // Step 4: Save changes
        await _dbContext.SaveChangesAsync();

        // Step 5: Return the updated order
        return existingOrder;
    }
}
