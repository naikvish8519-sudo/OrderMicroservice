﻿//using AutoMapper;
//using BusinessLogicLayer.Mappers;
//using eCommerce.OrdersMicroservice.BusinessLogicLayer.DTO;
//using eCommerce.OrdersMicroservice.BusinessLogicLayer.ServiceContracts;
//using eCommerce.OrdersMicroservice.DataAccessLayer.Entities;
//using eCommerce.OrdersMicroservice.DataAccessLayer.RepositoryContracts;
//using FluentValidation;
//using FluentValidation.Results;
//using MongoDB.Driver;
//using System.Linq.Expressions;

//namespace eCommerce.OrdersMicroservice.BusinessLogicLayer.Services;

//public class OrdersService : IOrdersService
//{
//    private readonly IValidator<OrderAddRequest> _orderAddRequestValidator;
//    private readonly IValidator<OrderItemAddRequest> _orderItemAddRequestValidator;
//    private readonly IValidator<OrderUpdateRequest> _orderUpdateRequestValidator;
//    private readonly IValidator<OrderItemUpdateRequest> _orderItemUpdateRequestValidator;
//    private readonly IMapper _mapper;
//    private readonly IOrdersRepository _ordersRepository;

//    public OrdersService(
//        IOrdersRepository ordersRepository,
//        IMapper mapper,
//        IValidator<OrderAddRequest> orderAddRequestValidator,
//        IValidator<OrderItemAddRequest> orderItemAddRequestValidator,
//        IValidator<OrderUpdateRequest> orderUpdateRequestValidator,
//        IValidator<OrderItemUpdateRequest> orderItemUpdateRequestValidator)
//    {
//        _orderAddRequestValidator = orderAddRequestValidator;
//        _orderItemAddRequestValidator = orderItemAddRequestValidator;
//        _orderUpdateRequestValidator = orderUpdateRequestValidator;
//        _orderItemUpdateRequestValidator = orderItemUpdateRequestValidator;
//        _mapper = mapper;
//        _ordersRepository = ordersRepository;
//    }

//    public async Task<OrderResponse?> AddOrder(OrderAddRequest orderAddRequest)
//    {
//        if (orderAddRequest == null) throw new ArgumentNullException(nameof(orderAddRequest));

//        ValidationResult validationResult = await _orderAddRequestValidator.ValidateAsync(orderAddRequest);
//        if (!validationResult.IsValid)
//        {
//            var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
//            throw new ArgumentException(errors);
//        }

//        foreach (var orderItemAddRequest in orderAddRequest.OrderItems)
//        {
//            var itemValidationResult = await _orderItemAddRequestValidator.ValidateAsync(orderItemAddRequest);
//            if (!itemValidationResult.IsValid)
//            {
//                var errors = string.Join(", ", itemValidationResult.Errors.Select(e => e.ErrorMessage));
//                throw new ArgumentException(errors);
//            }
//        }

//        // TODO: Check if UserID exists in Users microservice

//        var orderInput = _mapper.Map<Order>(orderAddRequest);

//        foreach (var orderItem in orderInput.OrderItems)
//        {
//            orderItem.TotalPrice = orderItem.Quantity * orderItem.UnitPrice;
//        }
//        orderInput.TotalBill = orderInput.OrderItems.Sum(i => i.TotalPrice);

//        var addedOrder = await _ordersRepository.AddOrder(orderInput);
//        if (addedOrder == null) return null;

//        return _mapper.Map<OrderResponse>(addedOrder);
//    }


//    public async Task<OrderResponse?> UpdateOrder(OrderUpdateRequest orderUpdateRequest)
//    {
//        // Check for null parameter
//        if (orderUpdateRequest == null)
//        {
//            throw new ArgumentNullException(nameof(orderUpdateRequest));
//        }

//        // Validate OrderUpdateRequest using FluentValidation
//        ValidationResult orderUpdateRequestValidationResult = await _orderUpdateRequestValidator.ValidateAsync(orderUpdateRequest);
//        if (!orderUpdateRequestValidationResult.IsValid)
//        {
//            string errors = string.Join(", ", orderUpdateRequestValidationResult.Errors.Select(error => error.ErrorMessage));
//            throw new ArgumentException(errors);
//        }

//        // Validate each OrderItemUpdateRequest using FluentValidation
//        foreach (OrderItemUpdateRequest orderItemUpdateRequest in orderUpdateRequest.OrderItems)
//        {
//            ValidationResult orderItemUpdateRequestValidationResult = await _orderItemUpdateRequestValidator.ValidateAsync(orderItemUpdateRequest);

//            if (!orderItemUpdateRequestValidationResult.IsValid)
//            {
//                string errors = string.Join(", ", orderItemUpdateRequestValidationResult.Errors.Select(error => error.ErrorMessage));
//                throw new ArgumentException(errors);
//            }
//        }

//        // TODO: Add logic for checking if UserID exists in Users microservice

//        // Convert data from OrderUpdateRequest to Order (DTO to Domain Model)
//        Order orderInput = _mapper.Map<Order>(orderUpdateRequest); // Uses OrderUpdateRequestToOrder mapping profile


//        //Order orderInput = OrderMapper.MapToOrder(orderUpdateRequest);

//        // Generate values for each order item
//        foreach (OrderItem orderItem in orderInput.OrderItems)
//        {
//            orderItem.TotalPrice = orderItem.Quantity * orderItem.UnitPrice;
//        }

//        // Calculate total bill
//        orderInput.TotalBill = orderInput.OrderItems.Sum(orderItem => orderItem.TotalPrice);

//        // Invoke repository to update the order
//        Order? updatedOrder = await _ordersRepository.UpdateOrder(orderInput);

//        // If no order was updated, return null
//        if (updatedOrder == null)
//        {
//            return null;
//        }

//        // Map updated Order entity to OrderResponse DTO
//        OrderResponse updatedOrderResponse = _mapper.Map<OrderResponse>(updatedOrder);

//        // Return the response
//        return updatedOrderResponse;
//    }


//    public async Task<bool> DeleteOrder(Guid orderID)
//    {
//        var existingOrder = await _ordersRepository.GetOrderByCondition(o => o.OrderID == orderID);
//        if (existingOrder == null) return false;

//        return await _ordersRepository.DeleteOrder(orderID);
//    }

//    public async Task<OrderResponse?> GetOrderByCondition(Expression<Func<Order, bool>> predicate)
//    {
//        var order = await _ordersRepository.GetOrderByCondition(predicate);
//        if (order == null) return null;

//        return _mapper.Map<OrderResponse>(order);
//    }

//    public async Task<List<OrderResponse?>> GetOrdersByCondition(Expression<Func<Order, bool>> predicate)
//    {
//        var orders = await _ordersRepository.GetOrdersByCondition(predicate);
//        var orderResponses = _mapper.Map<IEnumerable<OrderResponse?>>(orders);
//        return orderResponses.ToList();
//    }
//    public async Task<List<OrderResponse?>> GetOrders()
//    {
//        var orders = await _ordersRepository.GetOrders();
//        var orderResponses = _mapper.Map<IEnumerable<OrderResponse?>>(orders);
//        return orderResponses.ToList();
//    }
//}
