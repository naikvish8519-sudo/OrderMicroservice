using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerce.OrdersMicroservice.DataAccessLayer.Entities;

public class Order
{
    [Key]
    public Guid OrderID { get; set; }

    public Guid UserID { get; set; }

    public DateTime OrderDate { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalBill { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
