namespace ECommerceAPI.Domain;

public class Product {
    public int Id {get; set;}
    public string Name {get; set;}
    public string Description {get; set;}

    public decimal Price {get; set;}
    public int Quantity {get; set;}

    
    public List<Category> CategoryRisuto {get; set;} = new List<Category>();
    public List<CartItem> CartItemRisuto {get; set;} = new List<CartItem>();
    public List<OrderItem> OrderItemRisuto {get; set;} = new List<OrderItem>();
}