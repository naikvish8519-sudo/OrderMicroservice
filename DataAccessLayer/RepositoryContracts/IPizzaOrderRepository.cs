using eCommerce.OrdersMicroservice.DataAccessLayer.Entities;
using System.Linq.Expressions;

namespace eCommerce.OrdersMicroservice.DataAccessLayer.RepositoryContracts;
public interface IPizzaOrderRepository
{
    Task<PizzaOrder?> AddOrderAsync(PizzaOrder order);
    Task<IEnumerable<PizzaOrder>> GetAllOrdersAsync();
    Task<IEnumerable<PizzaOrder>> GetOrdersByUserAsync(Guid userId, bool onlyOrdered = true);
    Task<PizzaOrder?> GetOrderByIdAsync(Guid orderId);
    Task<bool> DeleteOrderAsync(Guid orderId);
    Task<PizzaOrder?> UpdateOrderAsync(PizzaOrder order);
    Task<PizzaOrder?> GetPizzaOrderByCondition(Expression<Func<PizzaOrder, bool>> predicate);
    Task<List<PizzaOrder>> GetPizzaOrdersByCondition(Expression<Func<PizzaOrder, bool>> predicate);

}
