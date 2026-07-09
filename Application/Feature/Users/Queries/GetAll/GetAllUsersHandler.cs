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
        IQueryable<User> source = _context.UserTable.AsNoTracking();

        if (!source.Any())
            throw new NotFoundException("No user found");

        var limit = Math.Min(query.Limit, 100);

        var arranged = await source
            .OrderBy(u => u.Id)
            .Skip((query.Page - 1) * limit)
            .Take(limit)
            .ToListAsync(cancellationToken);

        var result = _mapper.Map<IEnumerable<User>>(arranged);

        return new UserListResponse {Items = result};
    }
}


