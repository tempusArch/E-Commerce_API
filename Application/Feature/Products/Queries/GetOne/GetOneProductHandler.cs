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
            .Where(x => x.Id == query.Id)
            .Select(x => new ReadProductDto {
                ProductId = x.Id,
                ProductName = x.Name,
                ProductDescription = x.Description,
                Price = x.Price,
                CategoryRisuto = x.CategoryRisuto
                    .Select(z => new ReadCategoryDto {
                        CategoryId = z.Id,
                        CategoryName = z.Name
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (theOne == null)
            throw new NotFoundException("Product not found");

        return theOne;
    }
}
