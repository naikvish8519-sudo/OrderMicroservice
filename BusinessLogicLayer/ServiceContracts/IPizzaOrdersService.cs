using eCommerce.OrdersMicroservice.BusinessLogicLayer.DTO;
using eCommerce.OrdersMicroservice.DataAccessLayer.Entities;
using System.Linq.Expressions;

namespace eCommerce.OrdersMicroservice.BusinessLogicLayer.ServiceContracts;

public interface IPizzaOrdersService
{
    Task<List<PizzaOrderResponse?>> GetOrders();
    Task<List<PizzaOrderResponse?>> GetOrdersByCondition(Expression<Func<PizzaOrder, bool>> predicate);
    Task<PizzaOrderResponse?> GetOrderByCondition(Expression<Func<PizzaOrder, bool>> predicate);
    Task<PizzaOrderResponse?> AddPizzaOrder(PizzaOrderAddRequest pizzaOrderAddRequest);
    Task<PizzaOrderResponse?> UpdatePizzaOrder(PizzaOrderUpdateRequest pizzaOrderUpdateRequest);
    Task<bool> DeletePizzaOrder(Guid pizzaOrderID);
}
