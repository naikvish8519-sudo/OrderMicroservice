using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerce.OrdersMicroservice.DataAccessLayer.Entities;

public class OrderItem
{
    [Key]
    public Guid OrderItemID { get; set; }

    public Guid OrderID { get; set; } // Foreign key

    [ForeignKey("OrderID")]
    public Order? Order { get; set; }

    public Guid ProductID { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalPrice { get; set; }
}
