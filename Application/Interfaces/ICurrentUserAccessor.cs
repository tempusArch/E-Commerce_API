namespace ECommerceAPI.Application;

public interface ICurrentUserAccessor {
    string GetCurrentUserEmail();
    int GetCurrentUserId();
    Task<int> GetCurrentUserCartId();
}