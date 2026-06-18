using ECommerceAPI.Domain;

namespace ECommerceAPI.Application;

public class UpdateProductDto {
    public int Id {get; set;}

    public string? Name {get; set;}
    public string? Description {get; set;}

    public decimal Price {get; set;}
    public int Quantity {get; set;}
    
    public List<int> CategoryIdRisuto {get; set;} = new();
}