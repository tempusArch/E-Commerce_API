using ECommerceAPI.Domain;

namespace ECommerceAPI.Application;

public class ReadProductDto {
    public int ProductId {get; set;}
    
    public string? ProductName {get; set;}
    public string? ProductDescription {get; set;}
    
    public decimal Price {get; set;}
    public List<ReadCategoryDto> CategoryRisuto {get; set;} = new();
}