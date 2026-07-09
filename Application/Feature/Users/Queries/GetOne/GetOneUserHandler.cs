using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Domain;
using ECommerceAPI.Infrastructure;

namespace ECommerceAPI.Application;

public class GetOneUserHandler : IRequestHandler<GetOneUserQuery, User> {
    private readonly ECommerceApiDbContext _context;
    private readonly IMapper _mapper;
    public GetOneUserHandler(ECommerceApiDbContext context, IMapper mapper) {
        _context = context;
        _mapper = mapper;
    }

    public async Task<User> Handle(GetOneUserQuery query, CancellationToken cancellationToken) {
        var theOne = await _context.UserTable
            .AsNoTracking()
            .Include(u => u.OrderRisuto)
            .FirstOrDefaultAsync(u => u.Id == query.Id, cancellationToken);

        if (theOne == null)
            throw new NotFoundException("User not found");

        return theOne;
    }
}
