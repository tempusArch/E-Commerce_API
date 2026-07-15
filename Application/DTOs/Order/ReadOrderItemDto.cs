using ECommerceAPI.Domain;

namespace ECommerceAPI.Application;

public class ReadOrderItemDto {
    public int ProductId {get; set;}
    public string ProductName {get; set;} = null!;
    public string ProductDescription {get; set;} = null!;
    
    public decimal UnitPrice {get; set;} 
    public int Quantity {get; set;}
    public decimal TotalPrice => Quantity * UnitPrice; 
}