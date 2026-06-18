using System.Net.Http.Headers;

namespace ECommerceAPI.Domain;

public class CartItem {
    public int ProductId {get; set;}
    public Product Product {get; set;}

    public int CartId {get; set;}
    public Cart Cart {get; set;}

    public int Quantity {get; set;}
    public decimal TotalPrice => Product!.Price * Quantity;
}