using ECommerceAPI.Domain;

namespace ECommerceAPI.Application;

public class CreateProductDto {
    public string? Name {get; set;}
    public string? Description {get; set;}
    
    public decimal Price {get; set;}
    public int Quantity {get; set;}
    
    public List<int> CategoryIdRisuto {get; set;} = new();
}