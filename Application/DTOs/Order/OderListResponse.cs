namespace ECommerceAPI.Application;

public class OrderListResponse{
    public IEnumerable<ReadOrderDto> Items {get; set;} = new List<ReadOrderDto>();
  
    public decimal TotalPrice => Items.Sum(i => i.TotalPrice);
    public int TotalCount => Items.Count();

}