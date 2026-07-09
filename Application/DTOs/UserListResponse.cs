using ECommerceAPI.Domain;

namespace ECommerceAPI.Application;

public class UserListResponse {
    public IEnumerable<User> Items {get; set;} = new List<User>();
    public int TotalCount => Items.Count();
}