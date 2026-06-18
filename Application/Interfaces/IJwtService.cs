using ECommerceAPI.Domain;

namespace ECommerceAPI.Application;

public interface IJwtService {
    string Generate_JWT(User um);
    RefreshTokenModel Generate_RefreshToken(string UserId);
}