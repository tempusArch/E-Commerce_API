using ECommerceAPI.Domain;

namespace ECommerceAPI.Application;

public class UserListResponse {
    public IEnumerable<ReadUserDtoAdmin> Items {get; set;} = new List<ReadUserDtoAdmin>();
    public int TotalCount => Items.Count();
}