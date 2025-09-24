////using eCommerce.OrdersMicroservice.DataAccessLayer.Entities;
////using MongoDB.Driver;

////namespace eCommerce.OrdersMicroservice.DataAccessLayer.RepositoryContracts;

////public interface IOrdersRepository
////{
////  /// <summary>
////  /// Retrieves all Orders asynchronously
////  /// </summary>
////  /// <returns>Returns all orders from the orders collection</returns>
////  Task<IEnumerable<Order>> GetOrders();


////  /// <summary>
////  /// Retrieves all Orders based on the specified condition asynchronously
////  /// </summary>
////  /// <param name="filter">The condition to filter orders</param>
////  /// <returns>Returning a collection of matching orders</returns>
////  Task<IEnumerable<Order?>> GetOrdersByCondition(FilterDefinition<Order> filter);


////  /// <summary>
////  /// Retrieves a single order based on the specified condition asynchronously
////  /// </summary>
////  /// <param name="filter">The condition to filter Orders</param>
////  /// <returns>Returning matching order</returns>
////  Task<Order?> GetOrderByCondition(FilterDefinition<Order> filter);


////  /// <summary>
////  /// Adds a new Order into the Orders collection asynchronously
////  /// </summary>
////  /// <param name="order">The order to be added</param>
////  /// <returns>Returnes the added Order object or null if unsuccessful</returns>
////  Task<Order?> AddOrder(Order order);


////  /// <summary>
////  /// Updates an existing order asynchronously
////  /// </summary>
////  /// <param name="order">The order to be added</param>
////  /// <returns>Returns the updated order object; or null if not found</returns>
////  Task<Order?> UpdateOrder(Order order);


////  /// <summary>
////  /// Deletes the order asynchronously
////  /// </summary>
////  /// <param name="orderID">The Order ID based on which we need to delete the order</param>
////  /// <returns>Returns true if the deletion is successful, false otherwise</returns>
////  Task<bool> DeleteOrder(Guid orderID);
////}

//using eCommerce.OrdersMicroservice.DataAccessLayer.Entities;
//using System.Linq.Expressions;

//namespace eCommerce.OrdersMicroservice.DataAccessLayer.RepositoryContracts
//{
//    public interface IOrdersRepository
//    {
//        /// <summary>
//        /// Retrieves all orders asynchronously.
//        /// </summary>
//        Task<IEnumerable<Order>> GetOrders();

//        /// <summary>
//        /// Retrieves all orders matching the given condition.
//        /// </summary>
//        /// <param name="condition">LINQ expression for filtering orders</param>
//        Task<IEnumerable<Order>> GetOrdersByCondition(Expression<Func<Order, bool>> condition);

//        /// <summary>
//        /// Retrieves a single order matching the given condition.
//        /// </summary>
//        /// <param name="condition">LINQ expression for filtering orders</param>
//        Task<Order?> GetOrderByCondition(Expression<Func<Order, bool>> condition);

//        /// <summary>
//        /// Adds a new order to the database.
//        /// </summary>
//        /// <param name="order">Order to add</param>
//        Task<Order?> AddOrder(Order order);

//        /// <summary>
//        /// Updates an existing order.
//        /// </summary>
//        /// <param name="order">Order with updated data</param>
//        Task<Order?> UpdateOrder(Order order);

//        /// <summary>
//        /// Deletes an order by ID.
//        /// </summary>
//        /// <param name="orderID">The order ID to delete</param>
//        Task<bool> DeleteOrder(Guid orderID);
//    }
//}
