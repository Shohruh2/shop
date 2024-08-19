namespace Shop.Domain.Orders;

public class Order
{
    public required Guid Id { get; set; }
    
    public required DateTime DateTime { get; set; }
    

    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}