using ECommerceAPI.Domain;

namespace ECommerceAPI.Application;

public class RegisterUserDto {
    public string Name {get; set;} = null!;
    public string Email {get; set;}
    public string Password {get; set;}
    public string ConfirmPassword {get; set;}
}