using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerce.OrdersMicroservice.DataAccessLayer.Entities
{
    public class PizzaOrder
    {
        [Key]
        public Guid OrderID { get; set; } = Guid.NewGuid(); // Unique ID for this pizza order/cart item

        [Required]
        public Guid UserID { get; set; }                    // Associated user

        [Required]
        [MaxLength(50)]
        public string PizzaSize { get; set; } = string.Empty; // Size: Small, Medium, etc.

        [MaxLength(500)]
        public List<string> Toppings { get; set; } = new();
        

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal UnitPrice { get; set; }              // Price per pizza with toppings

        [Required]
        public int Quantity { get; set; }                   // Quantity of pizzas

        [NotMapped]
        public decimal TotalPrice => UnitPrice * Quantity;  // Computed in code, not persisted

        [Required]
        public bool IsOrdered { get; set; } = false;        // Cart item or confirmed order

        public DateTime? OrderDate { get; set; }            // Set only when order is placed

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow; // Timestamp when added to cart
    }
}
