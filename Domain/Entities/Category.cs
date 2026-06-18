namespace ECommerceAPI.Domain;

public class Category {
    public int Id {get; set;}
    public string Name {get; set;}
    public List<Product> ProductRisuto {get; set;} = new List<Product>();
}