using eCommerce.OrdersMicroservice.BusinessLogicLayer.DTO;
using eCommerce.OrdersMicroservice.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;

namespace eCommerce.OrdersMicroservice.BusinessLogicLayer.Mappers
{
    public static class PizzaOrderMapper
    {
        public static PizzaOrder MapToPizzaOrder(PizzaOrderAddRequest request)
        {
            return new PizzaOrder
            {
                OrderID = Guid.NewGuid(),
                UserID = request.UserID,
                PizzaSize = request.PizzaSize,
                Toppings = request.Toppings,
                UnitPrice = request.UnitPrice,
                Quantity = request.Quantity,
                IsOrdered = request.IsOrdered,
                OrderDate = DateTime.UtcNow
                // TotalPrice is computed in property getter
            };
        }

        public static PizzaOrder MapToPizzaOrder(PizzaOrderUpdateRequest request)
        {
            return new PizzaOrder
            {
                OrderID = request.OrderID,
                UserID = request.UserID,
                PizzaSize = request.PizzaSize,
                Toppings = request.Toppings,
                UnitPrice = request.UnitPrice,
                Quantity = request.Quantity,
                IsOrdered = request.IsOrdered,
              
                // TotalPrice is computed in property getter
            };
        }

        public static PizzaOrderResponse MapToPizzaOrderResponse(PizzaOrder entity)
        {
            return new PizzaOrderResponse
            {
                OrderID = entity.OrderID,
                UserID = entity.UserID,
                PizzaSize = entity.PizzaSize,
                Toppings = entity.Toppings,
                UnitPrice = entity.UnitPrice,
                Quantity = entity.Quantity,
                TotalPrice = entity.TotalPrice,
                IsOrdered = entity.IsOrdered,
                
            };
        }

        public static List<PizzaOrderResponse> MapToPizzaOrderResponseList(IEnumerable<PizzaOrder> entities)
        {
            var responses = new List<PizzaOrderResponse>();
            foreach (var entity in entities)
            {
                responses.Add(MapToPizzaOrderResponse(entity));
            }
            return responses;
        }
    }
}
