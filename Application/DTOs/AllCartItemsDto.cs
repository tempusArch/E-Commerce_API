using ECommerceAPI.Domain;

namespace ECommerceAPI.Application;

public class AllCartItemsDto {
    public AllCartItemsDto(List<SingleCartItemDto> cartItems)
    {
        Items = cartItems;
    }
    public List<SingleCartItemDto> Items { get; set; }
   
    public decimal TotalPrice => Items.Sum(i => i.TotalPrice);
}