using MediatR;
using AutoMapper;
using ECommerceAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Application;

public class GetOneProductHandler : IRequestHandler<GetOneProductQuery, ReadProductDto> {
    private readonly ECommerceApiDbContext _context;
    private readonly IMapper _mapper;
    public GetOneProductHandler(ECommerceApiDbContext context, IMapper mapper) {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ReadProductDto> Handle(GetOneProductQuery query, CancellationToken cancellationToken) {
        var theOne = await _context.ProductTable
            .AsNoTracking()
            .Include(p => p.CategoryRisuto)
            .FirstOrDefaultAsync(p => p.Id == query.Id, cancellationToken);

        if (theOne == null)
            throw new NotFoundException("Product not found");

        return _mapper.Map<ReadProductDto>(theOne);
    }
}
