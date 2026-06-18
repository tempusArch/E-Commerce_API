using System.Globalization;
using System.Security.Claims;
using ECommerceAPI.Application;
using ECommerceAPI.Domain;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Infrastructure;

public class CurrentUserAccessor : ICurrentUserAccessor {
    private readonly ECommerceApiDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CurrentUserAccessor(ECommerceApiDbContext context, IHttpContextAccessor httpContextAccessor) {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetCurrentUserEmail() {
        return _httpContextAccessor.HttpContext!.User.FindFirst(ClaimTypes.Email)!.Value;
    }

    public int GetCurrentUserId() {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext == null || httpContext.User == null || !httpContext.User.Claims.Any())
            return 0;

        var claim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);

        if (claim == null)
            return 0;

        return int.Parse(claim.Value);
    }

    public async Task<int> GetCurrentUserCartId() {
        var userId = int.Parse(_httpContextAccessor.HttpContext!.User
            .FindFirst(ClaimTypes.NameIdentifier)!.Value);

        Cart? cart = await _context.CartTable.AsNoTracking()
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
            throw new Exception("User does not have a cart");

        return cart.Id;
    }
}