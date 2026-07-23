using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Domain;
using ECommerceAPI.Infrastructure;

namespace ECommerceAPI.Application;

public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, UserListResponse> {
    private readonly ECommerceApiDbContext _context;
    private readonly IMapper _mapper;
    public GetAllUsersHandler(ECommerceApiDbContext context, IMapper mapper) {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserListResponse> Handle(GetAllUsersQuery query, CancellationToken cancellationToken) {
        var limit = Math.Min(query.Limit, 100);
        
        var result = await _context.UserTable
            .AsNoTracking()
            .Select(x => new ReadUserDtoAdmin {
                UserId = x.Id,
                UserName = x.Name,
                Email = x.Email,
                CartId = x.CartId,
                CartItemRisuto = x.Cart.CartItemRisuto,
                OrderRisuto = x.OrderRisuto,
                Role = x.Role
            })
            .OrderBy(x => x.Role)
            .ThenBy(x => x.UserId)
            .Skip((query.Page - 1) * limit)
            .Take(limit)
            .ToListAsync(cancellationToken);

        return new UserListResponse {Items = result};
    }
}


