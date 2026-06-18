using ECommerceAPI.Domain;

namespace ECommerceAPI.Application;

public class OrderDto {
    //public int Id {get; set;}

    public AllCartItemsDto AllCartItems {get; set;}

    public int OrderId {get; set;}
    public OrderStatus OrderStatus {get; set;}
    
    public string? PaymentIntenId {get; set;}
    //public string? ClientSecret {get; set;}

    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;
}