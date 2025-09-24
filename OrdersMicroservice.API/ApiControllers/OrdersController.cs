//using eCommerce.OrdersMicroservice.BusinessLogicLayer.DTO;
//using eCommerce.OrdersMicroservice.BusinessLogicLayer.ServiceContracts;
//using eCommerce.OrdersMicroservice.DataAccessLayer.Entities;
//using Microsoft.AspNetCore.Mvc;
//using System.Linq.Expressions;
//using BusinessLogicLayer.HttpClients;

//namespace OrdersMicroservice.API.ApiControllers;

//[Route("api/[controller]")]
//[ApiController]
//public class OrdersController : ControllerBase
//{
//    private readonly IOrdersService _ordersService;
//    private readonly UsersMicroserviceClient _usersClient;
//    private readonly ProductMicroserviceClient _productsClient;

//    public OrdersController(
//        IOrdersService ordersService,
//        UsersMicroserviceClient usersClient,
//        ProductMicroserviceClient productClient)
//    {
//        _ordersService = ordersService;
//        _productsClient = productClient;
//        _usersClient = usersClient;
//    }

//    // GET: /api/Orders
//    [HttpGet]
//    public async Task<IActionResult> Get()
//    {
//        try
//        {
//            List<OrderResponse?> orders = await _ordersService.GetOrders();
//            return Ok(orders);
//        }
//        catch (Exception ex)
//        {
//            return Problem($"Error fetching orders: {ex.Message}");
//        }
//    }

//    // GET: /api/Orders/search/orderid/{orderID}
//    [HttpGet("search/orderid/{orderID}")]
//    public async Task<IActionResult> GetOrderByOrderID(Guid orderID)
//    {
//        try
//        {
//            Expression<Func<Order, bool>> filter = o => o.OrderID == orderID;
//            OrderResponse? order = await _ordersService.GetOrderByCondition(filter);

//            if (order == null)
//                return NotFound($"Order {orderID} not found.");

//            return Ok(order);
//        }
//        catch (Exception ex)
//        {
//            return Problem($"Error fetching order by ID: {ex.Message}");
//        }
//    }

//    // GET: /api/Orders/search/productid/{productID}
//    [HttpGet("search/productid/{productID}")]
//    public async Task<IActionResult> GetOrdersByProductID(Guid productID)
//    {
//        try
//        {
//            Expression<Func<Order, bool>> filter = o => o.OrderItems.Any(oi => oi.ProductID == productID);
//            List<OrderResponse?> orders = await _ordersService.GetOrdersByCondition(filter);

//            return Ok(orders);
//        }
//        catch (Exception ex)
//        {
//            return Problem($"Error fetching orders by product ID: {ex.Message}");
//        }
//    }

//    // GET: /api/Orders/search/orderDate/{orderDate}
//    [HttpGet("search/orderDate/{orderDate}")]
//    public async Task<IActionResult> GetOrdersByOrderDate(DateTime orderDate)
//    {
//        try
//        {
//            Expression<Func<Order, bool>> filter = o => o.OrderDate.Date == orderDate.Date;
//            List<OrderResponse?> orders = await _ordersService.GetOrdersByCondition(filter);

//            return Ok(orders);
//        }
//        catch (Exception ex)
//        {
//            return Problem($"Error fetching orders by date: {ex.Message}");
//        }
//    }

//    // POST: /api/Orders
//    [HttpPost]
//    public async Task<IActionResult> Post(OrderAddRequest orderAddRequest)
//    {
//        try
//        {
//            if (orderAddRequest is null)
//                return BadRequest("Invalid order data");

//            if (!ModelState.IsValid)
//                return ValidationProblem(ModelState);

//            if (orderAddRequest.UserId == Guid.Empty)
//                return BadRequest("UserId is required.");

//            var user = await _usersClient.GetUserByIdAsync(orderAddRequest.UserId);
//            if (user is null)
//                return NotFound($"User {orderAddRequest.UserId} not found.");

//            foreach (var item in orderAddRequest.OrderItems)
//            {
//                var product = await _productsClient.GetProductByIdAsync(item.ProductID);
//                if (product is null)
//                    return NotFound($"Product {item.ProductID} not found.");
//            }

//            var orderResponse = await _ordersService.AddOrder(orderAddRequest);
//            if (orderResponse is null)
//                return Problem("Error in adding order");

//            return Created($"api/Orders/search/orderid/{orderResponse.OrderID}", orderResponse);
//        }
//        catch (Exception ex)
//        {
//            return Problem($"Error creating order: {ex.Message}");
//        }
//    }

//    // PUT: /api/Orders/{orderID}
//    [HttpPut("{orderID}")]
//    public async Task<IActionResult> Put(Guid orderID, OrderUpdateRequest orderUpdateRequest)
//    {
//        try
//        {
//            if (orderUpdateRequest == null)
//                return BadRequest("Invalid order data");

//            if (orderID != orderUpdateRequest.OrderID)
//                return BadRequest("OrderID in the URL doesn't match with the OrderID in the Request body");

//            foreach (var item in orderUpdateRequest.OrderItems)
//            {
//                var product = await _productsClient.GetProductByIdAsync(item.ProductID);
//                if (product is null)
//                    return NotFound($"Product {item.ProductID} not found.");
//            }

//            OrderResponse? orderResponse = await _ordersService.UpdateOrder(orderUpdateRequest);

//            if (orderResponse == null)
//                return Problem("Error in updating order");

//            return Ok(orderResponse);
//        }
//        catch (Exception ex)
//        {
//            return Problem($"Error updating order: {ex.Message}");
//        }
//    }

//    // DELETE: /api/Orders/{orderID}
//    [HttpDelete("{orderID}")]
//    public async Task<IActionResult> Delete(Guid orderID)
//    {
//        try
//        {
//            if (orderID == Guid.Empty)
//                return BadRequest("Invalid order ID");

//            bool isDeleted = await _ordersService.DeleteOrder(orderID);

//            if (!isDeleted)
//                return Problem("Error in deleting order");

//            return Ok(isDeleted);
//        }
//        catch (Exception ex)
//        {
//            return Problem($"Error deleting order: {ex.Message}");
//        }
//    }
//}
