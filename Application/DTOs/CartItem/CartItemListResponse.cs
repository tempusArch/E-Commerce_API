using ECommerceAPI.Domain;

namespace ECommerceAPI.Application;

public class CartItemListResponse{
    public IEnumerable<ReadCartItemDto> Items {get; set;} = new List<ReadCartItemDto>();
  
    public decimal TotalPrice => Items.Sum(i => i.TotalPrice);
    public int TotalCount => Items.Count();

}