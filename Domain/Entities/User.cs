namespace ECommerceAPI.Domain;

public class User {
    public int Id {get; set;}

    public string Name {get; set;}
    public string Email {get; set;}
    public string PasswordHashed {get; set;}

    public int CartId {get; set;}
    public Cart? Cart {get; set;}
    public List<Order> OrderRisuto {get; set;} = new List<Order>();

    public string Role {get; set;} = "User";
}