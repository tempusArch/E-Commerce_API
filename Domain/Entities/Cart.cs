namespace ECommerceAPI.Domain;

public class Cart {
    public int Id {get; set;}
    
    public int UserId {get; set;}
    public User User {get; set;}

    public List<CartItem> CartItemRisuto {get; set;} = new List<CartItem>();
    public decimal TotalPrice => CartItemRisuto.Sum(n => n.TotalPrice);

    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;
    public DateTime UpdatedAt {get; set;} = DateTime.UtcNow;
}