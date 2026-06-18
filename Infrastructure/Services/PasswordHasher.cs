using ECommerceAPI.Application;
using ECommerceAPI.Domain;

namespace ECommerceAPI.Infrastructure;

public class PasswordHasher : IPasswordHasher {
    public string HashPassword(string m) {
        return BCrypt.Net.BCrypt.HashPassword(m);
    }

    public bool VerifyPassword(string enteredPassword, string hashedPassword) {
        return BCrypt.Net.BCrypt.Verify(enteredPassword, hashedPassword);
    }
}