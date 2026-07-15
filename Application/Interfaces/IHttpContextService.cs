using ECommerceAPI.Domain;

namespace ECommerceAPI.Application;

public interface IHttpContextService {
    string GetCurrentUserEmail();
    void CheckUserIdClaim();
    int GetCurrentUserId();
    string? GetRefreshToken();
    void SetRefreshToken(RefreshTokenModel newRefreshToken);
    void DeleteRefreshToken();
}