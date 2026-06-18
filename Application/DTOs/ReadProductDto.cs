using ECommerceAPI.Domain;

namespace ECommerceAPI.Application;

public class ReadProductDto {
    //public int Id {get; set;}
    
    public string? Name {get; set;}
    public string? Description {get; set;}
    
    public decimal Price {get; set;}
    public List<ReadCategoryDto> CategoryRisuto {get; set;} = new();
}