using eCommerce.OrdersMicroservice.BusinessLogicLayer.DTO;
using eCommerce.OrdersMicroservice.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Mappers
{
    public static class OrderMapper
    {

        public static Order MapToOrder(OrderUpdateRequest request)
        {
            var order = new Order
            {
                OrderID = request.OrderID,
                UserID = request.UserID,
                OrderDate = request.OrderDate,
                OrderItems = request.OrderItems.Select(item => new OrderItem
                {
                    OrderItemID = item.OrderItemID,
                    ProductID = item.ProductID,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    TotalPrice = item.UnitPrice * item.Quantity // calculate manually
                }).ToList()
            };

            order.TotalBill = order.OrderItems.Sum(i => i.TotalPrice);

            return order;
        }
    }
}
