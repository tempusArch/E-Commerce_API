using ECommerceAPI.Domain;

namespace ECommerceAPI.Application;

public class ReadOrderDto {
    public int OrderId {get; set;}

    public decimal TotalPrice {get; set;}
    public string? PaymentIntentId {get; set;}
    public OrderStatus OrderStatus {get; set;}   

    public List<ReadOrderItemDto> OrderItemRisuto {get; set;} = new();

    public DateTime CreatedAt {get; set;} 
    public DateTime UpdatedAt {get; set;}
}