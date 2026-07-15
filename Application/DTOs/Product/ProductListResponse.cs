using ECommerceAPI.Domain;

namespace ECommerceAPI.Application;

public class ProductListResponse {
    public IEnumerable<ReadProductDto> Items {get; set;} = new List<ReadProductDto>();
    public int TotalCount => Items.Count();
}