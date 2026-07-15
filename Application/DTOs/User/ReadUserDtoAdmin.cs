using ECommerceAPI.Domain;

namespace ECommerceAPI.Application;

public class ReadUserDtoAdmin {
    public int UserId {get; set;}

    public string UserName {get; set;}
    public string Email {get; set;}

    public int CartId {get; set;}
    public List<CartItem> CartItemRisuto {get; set;} = new List<CartItem>();
    
    public List<Order> OrderRisuto {get; set;} = new List<Order>();

    public string Role {get; set;}
}