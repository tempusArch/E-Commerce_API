using ECommerceAPI.Domain;

namespace ECommerceAPI.Application;

public class CategoryListResponse {
    public IEnumerable<ReadCategoryDto> Items {get; set;} = new List<ReadCategoryDto>();
    public int TotalCount => Items.Count();
}