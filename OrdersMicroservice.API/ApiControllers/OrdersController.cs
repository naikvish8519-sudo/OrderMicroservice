﻿using eCommerce.OrdersMicroservice.BusinessLogicLayer.DTO;
using eCommerce.OrdersMicroservice.BusinessLogicLayer.ServiceContracts;
using eCommerce.OrdersMicroservice.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Linq.Expressions;
using BusinessLogicLayer.HttpClients;


namespace OrdersMicroservice.API.ApiControllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
  private readonly IOrdersService _ordersService;
  private readonly UsersMicroserviceClient _usersClient;
  private readonly ProductMicroserviceClient _productsClient;     



    public OrdersController(IOrdersService ordersService, UsersMicroserviceClient usersClient, ProductMicroserviceClient productClient)
  {
    _ordersService = ordersService;
        _productsClient = productClient;
    _usersClient = usersClient;
    }


  //GET: /api/Orders
  [HttpGet]
  public async Task<IEnumerable<OrderResponse?>> Get()
  {
    List<OrderResponse?> orders = await _ordersService.GetOrders();
    return orders;
  }


  //GET: /api/Orders/search/orderid/{orderID}
  //[HttpGet("search/orderid/{orderID}")]
  //public async Task<OrderResponse?> GetOrderByOrderID(Guid orderID)
  //{
  //  FilterDefinition<Order> filter = Builders<Order>.Filter.Eq(temp => temp.OrderID, orderID);

  //  OrderResponse? order = await _ordersService.GetOrderByCondition(filter);
  //  return order;
  //}

    [HttpGet("search/orderid/{orderID}")]
    public async Task<OrderResponse?> GetOrderByOrderID(Guid orderID)
    {
        // Use a LINQ expression to filter by orderID
        Expression<Func<Order, bool>> filter = o => o.OrderID == orderID;

        OrderResponse? order = await _ordersService.GetOrderByCondition(filter);
        return order;
    }


    //GET: /api/Orders/search/productid/{productID}
    //  [HttpGet("search/productid/{productID}")]
    //public async Task<IEnumerable<OrderResponse?>> GetOrdersByProductID(Guid productID)
    //{
    //  FilterDefinition<Order> filter = Builders<Order>.Filter.ElemMatch(temp => temp.OrderItems, 
    //    Builders<OrderItem>.Filter.Eq(tempProduct => tempProduct.ProductID, productID)
    //    );

    //  List<OrderResponse?> orders = await _ordersService.GetOrdersByCondition(filter);
    //  return orders;
    //}
    [HttpGet("search/productid/{productID}")]
    public async Task<IEnumerable<OrderResponse?>> GetOrdersByProductID(Guid productID)
    {
        // Expression to check if any OrderItem's ProductID matches the given productID
        Expression<Func<Order, bool>> filter = o => o.OrderItems.Any(oi => oi.ProductID == productID);

        List<OrderResponse?> orders = await _ordersService.GetOrdersByCondition(filter);
        return orders;
    }



    //GET: /api/Orders/search/orderDate/{orderDate}
    //[HttpGet("/search/orderDate/{orderDate}")]
    //public async Task<IEnumerable<OrderResponse?>> GetOrdersByOrderDate(DateTime orderDate)
    //{
    //  FilterDefinition<Order> filter = Builders<Order>.Filter.Eq(temp => temp.OrderDate.ToString("yyyy-MM-dd"), orderDate.ToString("yyyy-MM-dd")
    //    );

    //  List<OrderResponse?> orders = await _ordersService.GetOrdersByCondition(filter);
    //  return orders;
    //}
    [HttpGet("/search/orderDate/{orderDate}")]
    public async Task<IEnumerable<OrderResponse?>> GetOrdersByOrderDate(DateTime orderDate)
    {
        // Create a LINQ expression filter instead of MongoDB FilterDefinition
        Expression<Func<Order, bool>> filter = o =>
            o.OrderDate.Date == orderDate.Date; // Compare Date only, ignore time

        List<OrderResponse?> orders = await _ordersService.GetOrdersByCondition(filter);
        return orders;
    }

    //POST api/Orders
    [HttpPost]
    public async Task<IActionResult> Post(OrderAddRequest orderAddRequest)
    {
        if (orderAddRequest is null)
            return BadRequest("Invalid order data");

        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        if (orderAddRequest.UserId == Guid.Empty)
            return BadRequest("UserId is required.");

        // ✅ Validate user exists in Users microservice
        var user = await _usersClient.GetUserByIdAsync(orderAddRequest.UserId);
        if (user is null)
            return NotFound($"User {orderAddRequest.UserId} not found.");

        foreach (var item in orderAddRequest.OrderItems)
        {
            var product = await _productsClient.GetProductByIdAsync(item.ProductID);
            if (product is null)
                return NotFound($"Product {item.ProductID} not found.");
        }

        // (Optional) add more checks, e.g. is user active/verified if your DTO supports it
        // if (user.IsActive == false) return Forbid("User is not active.");

        // Proceed with order creation
        var orderResponse = await _ordersService.AddOrder(orderAddRequest);
        if (orderResponse is null)
            return Problem("Error in adding order");

        return Created($"api/Orders/search/orderid/{orderResponse.OrderID}", orderResponse);
    }
    
    
    //PUT api/Orders/{orderID}
    [HttpPut("{orderID}")]
  public async Task<IActionResult> Put(Guid orderID, OrderUpdateRequest orderUpdateRequest)
  {

        foreach (var item in orderUpdateRequest.OrderItems)
        {
            Console.WriteLine($"OrderItemID: {item.OrderItemID}");
        }

        if (orderUpdateRequest == null)
    {
      return BadRequest("Invalid order data");
    }

    if (orderID != orderUpdateRequest.OrderID)
    {
      return BadRequest("OrderID in the URL doesn't match with the OrderID in the Request body");
    }

        foreach (var item in orderUpdateRequest.OrderItems)
        {
            var product = await _productsClient.GetProductByIdAsync(item.ProductID);
            if (product is null)
                return NotFound($"Product {item.ProductID} not found.");
        }


        OrderResponse? orderResponse = await _ordersService.UpdateOrder(orderUpdateRequest);

    if (orderResponse == null)
    {
      return Problem("Error in updating order");
    }


    return Ok(orderResponse);
  }


  //DELETE api/Orders/{orderID}
  [HttpDelete("{orderID}")]
  public async Task<IActionResult> Delete(Guid orderID)
  {
    if (orderID == Guid.Empty)
    {
      return BadRequest("Invalid order ID");
    }

    bool isDeleted = await _ordersService.DeleteOrder(orderID);

    if (!isDeleted)
    {
      return Problem("Error in deleting order");
    }

    return Ok(isDeleted);
  }
}
