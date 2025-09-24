using eCommerce.OrdersMicroservice.BusinessLogicLayer.DTO;
using eCommerce.OrdersMicroservice.BusinessLogicLayer.ServiceContracts;
using eCommerce.OrdersMicroservice.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace OrdersMicroservice.API.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PizzaOrdersController : ControllerBase
    {
        private readonly IPizzaOrdersService _pizzaOrdersService;

        public PizzaOrdersController(IPizzaOrdersService pizzaOrdersService)
        {
            _pizzaOrdersService = pizzaOrdersService;
        }

        // GET: api/PizzaOrders
        [HttpGet]
        [Produces("application/json")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var orders = await _pizzaOrdersService.GetOrders();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return Problem($"Error fetching pizza orders: {ex.Message}");
            }
        }

        // GET: api/PizzaOrders/search/orderid/{orderId}
        [HttpGet("search/orderid/{orderId}")]
        public async Task<IActionResult> GetByOrderId(Guid orderId)
        {
            try
            {
                Expression<Func<PizzaOrder, bool>> predicate = o => o.OrderID == orderId;
                var result = await _pizzaOrdersService.GetOrderByCondition(predicate);

                if (result == null)
                    return NotFound($"Order with ID {orderId} not found.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Problem($"Error fetching order by ID: {ex.Message}");
            }
        }

        // GET: api/PizzaOrders/search/userid/{userId}
        [HttpGet("search/userid/{userId}")]
        [Produces("application/json")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            try
            {
                Expression<Func<PizzaOrder, bool>> predicate = o => o.UserID == userId;
                var orders = await _pizzaOrdersService.GetOrdersByCondition(predicate);

                if (orders == null || !orders.Any())
                    return NotFound(new { message = $"No orders found for user {userId}" });

                return Ok(orders);
            }
            catch (Exception ex)
            {
                return Problem($"Error fetching orders by user ID: {ex.Message}");
            }
        }

        // GET: api/PizzaOrders/search/orderDate/{orderDate}
        [HttpGet("search/orderDate/{orderDate}")]
        public async Task<IActionResult> GetByOrderDate(DateTime orderDate)
        {
            try
            {
                Expression<Func<PizzaOrder, bool>> predicate = o =>
                    o.OrderDate.HasValue && o.OrderDate.Value.Date == orderDate.Date;

                var orders = await _pizzaOrdersService.GetOrdersByCondition(predicate);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return Problem($"Error fetching orders by date: {ex.Message}");
            }
        }

        // POST: api/PizzaOrders
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PizzaOrderAddRequest request)
        {
            try
            {
                if (request == null)
                    return BadRequest("Request cannot be null.");

                var result = await _pizzaOrdersService.AddPizzaOrder(request);
                if (result == null)
                    return Problem("Failed to create pizza order.");

                return CreatedAtAction(nameof(GetByOrderId), new { orderId = result.OrderID }, result);
            }
            catch (Exception ex)
            {
                return Problem($"Error creating pizza order: {ex.Message}");
            }
        }

        // PUT: api/PizzaOrders/{orderId}
        [HttpPut("{orderId}")]
        public async Task<IActionResult> Put(Guid orderId, [FromBody] PizzaOrderUpdateRequest request)
        {
            try
            {
                if (request == null)
                    return BadRequest("Request cannot be null.");

                if (orderId != request.OrderID)
                    return BadRequest("OrderID mismatch between URL and request body.");

                var updated = await _pizzaOrdersService.UpdatePizzaOrder(request);
                if (updated == null)
                    return Problem("Failed to update pizza order.");

                return Ok(updated);
            }
            catch (Exception ex)
            {
                return Problem($"Error updating pizza order: {ex.Message}");
            }
        }

        // DELETE: api/PizzaOrders/{orderId}
        [HttpDelete("{orderId}")]
        public async Task<IActionResult> Delete(Guid orderId)
        {
            try
            {
                if (orderId == Guid.Empty)
                    return BadRequest("Invalid Order ID.");

                bool deleted = await _pizzaOrdersService.DeletePizzaOrder(orderId);
                if (!deleted)
                    return Problem("Failed to delete pizza order.");

                return Ok(deleted);
            }
            catch (Exception ex)
            {
                return Problem($"Error deleting pizza order: {ex.Message}");
            }
        }
    }
}
