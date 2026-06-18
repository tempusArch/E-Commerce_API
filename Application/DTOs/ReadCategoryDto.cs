using ECommerceAPI.Domain;

namespace ECommerceAPI.Application;

public class ReadCategoryDto {  
    public string Name {get; set;}
    public List<ReadProductDto> ProductRisuto {get; set;} = new();
}