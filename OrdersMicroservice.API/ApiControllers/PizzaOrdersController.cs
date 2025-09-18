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
        public async Task<IEnumerable<PizzaOrderResponse?>> Get()
        {
            var orders = await _pizzaOrdersService.GetOrders();
            return orders;
        }

        // GET: api/PizzaOrders/search/orderid/{orderId}
        [HttpGet("search/orderid/{orderId}")]
        public async Task<ActionResult<PizzaOrderResponse?>> GetByOrderId(Guid orderId)
        {
            Expression<Func<PizzaOrder, bool>> predicate = o => o.OrderID == orderId;
            var result = await _pizzaOrdersService.GetOrderByCondition(predicate);

            if (result == null)
                return NotFound($"Order with ID {orderId} not found.");

            return Ok(result);
        }

        // GET: api/PizzaOrders/search/userid/{userId}
        [HttpGet("search/userid/{userId}")]
        public async Task<IEnumerable<PizzaOrderResponse?>> GetByUserId(Guid userId)
        {
            Expression<Func<PizzaOrder, bool>> predicate = o => o.UserID == userId;
            return await _pizzaOrdersService.GetOrdersByCondition(predicate);
        }

        [HttpGet("search/orderDate/{orderDate}")]
        public async Task<IEnumerable<PizzaOrderResponse?>> GetByOrderDate(DateTime orderDate)
        {
            // Safely compare nullable DateTime (if applicable)
            Expression<Func<PizzaOrder, bool>> predicate = o =>
                o.OrderDate.HasValue && o.OrderDate.Value.Date == orderDate.Date;

            var orders = await _pizzaOrdersService.GetOrdersByCondition(predicate);

            return orders;
        }


        // POST: api/PizzaOrders
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PizzaOrderAddRequest request)
        {
            if (request == null)
                return BadRequest("Request cannot be null.");

            var result = await _pizzaOrdersService.AddPizzaOrder(request);
            if (result == null)
                return Problem("Failed to create pizza order.");

            return CreatedAtAction(nameof(GetByOrderId), new { orderId = result.OrderID }, result);
        }

        // PUT: api/PizzaOrders/{orderId}
        [HttpPut("{orderId}")]
        public async Task<IActionResult> Put(Guid orderId, [FromBody] PizzaOrderUpdateRequest request)
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

        // DELETE: api/PizzaOrders/{orderId}
        [HttpDelete("{orderId}")]
        public async Task<IActionResult> Delete(Guid orderId)
        {
            if (orderId == Guid.Empty)
                return BadRequest("Invalid Order ID.");

            bool deleted = await _pizzaOrdersService.DeletePizzaOrder(orderId);
            if (!deleted)
                return Problem("Failed to delete pizza order.");

            return Ok(deleted);
        }
    }
}
