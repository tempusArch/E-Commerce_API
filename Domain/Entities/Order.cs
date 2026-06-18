namespace ECommerceAPI.Domain;

public class Order {
    public int Id {get; set;}

    public decimal TotalPrice {get; set;}
    public string? PaymentIntentId {get; set;}
    public OrderStatus OrderStatus {get; set;} = OrderStatus.Paid;

    public int UserId {get; set;}
    public User User {get; set;}

    public List<OrderItem> OrderItemRisuto {get; set;} = new List<OrderItem>();

    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;
    public DateTime UpdatedAt {get; set;} = DateTime.UtcNow;
}