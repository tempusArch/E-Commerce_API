using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Domain;
using ECommerceAPI.Infrastructure;

namespace ECommerceAPI.Application;

public class GetOneUserHandler : IRequestHandler<GetOneUserQuery, ReadUserDtoAdmin> {
    private readonly ECommerceApiDbContext _context;
    private readonly IMapper _mapper;
    public GetOneUserHandler(ECommerceApiDbContext context, IMapper mapper) {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ReadUserDtoAdmin> Handle(GetOneUserQuery query, CancellationToken cancellationToken) {
        var theOne = await _context.UserTable
            .AsNoTracking()
            .Where(x => x.Id == query.Id)
            .Select(x => new ReadUserDtoAdmin {
                UserId = x.Id,
                UserName = x.Name,
                Email = x.Email,
                CartId = x.CartId,
                CartItemRisuto = x.Cart.CartItemRisuto,
                OrderRisuto = x.OrderRisuto,
                Role = x.Role
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (theOne == null)
            throw new NotFoundException("User not found");

        return theOne;
    }
}
