using ECommerceAPI.Domain;

namespace ECommerceAPI.Application;

public interface IPasswordHasher {
    string HashPassword(string m);
    bool VerifyPassword(string hashedPassword, string enteredPassword);
}