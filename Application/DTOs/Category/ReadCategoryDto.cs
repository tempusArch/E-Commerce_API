using ECommerceAPI.Domain;

namespace ECommerceAPI.Application;

public class ReadCategoryDto { 
    public int CategoryId {get; set;} 
    public string CategoryName {get; set;}
    public List<ReadProductDto> ProductRisuto {get; set;} = new();
}