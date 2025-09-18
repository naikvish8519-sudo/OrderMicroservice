using System.ComponentModel.DataAnnotations;

namespace eCommerce.OrdersMicroservice.BusinessLogicLayer.DTO;

public class PizzaOrderAddRequest
{
    [Required]
    public Guid UserID { get; set; }

    [Required]
    [StringLength(50)]
    public string PizzaSize { get; set; } = string.Empty;

    [Required]
    public List<string> Toppings { get; set; } = new();

    [Required]
    public decimal UnitPrice { get; set; }

    [Required]
    public int Quantity { get; set; }

    public bool IsOrdered { get; set; } = false;
}
