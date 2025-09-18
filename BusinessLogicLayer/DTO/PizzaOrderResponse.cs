namespace eCommerce.OrdersMicroservice.BusinessLogicLayer.DTO;

public class PizzaOrderResponse
{
    public Guid OrderID { get; set; }

    public Guid UserID { get; set; }

    public DateTime OrderDate { get; set; }

    public string PizzaSize { get; set; } = string.Empty;

    public List<string> Toppings { get; set; } = new();

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public decimal TotalPrice { get; set; }

    public bool IsOrdered { get; set; }
}
