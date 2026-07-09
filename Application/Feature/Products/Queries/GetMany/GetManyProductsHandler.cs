using MediatR;
using AutoMapper;
using ECommerceAPI.Application;
using ECommerceAPI.Infrastructure;
using ECommerceAPI.Domain;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Application;

public class GetManyProductsHandler : IRequestHandler<GetManyProductsQuery, ProductListResponse> {
    private readonly ECommerceApiDbContext _context;
    private readonly IMapper _mapper;
    public GetManyProductsHandler(ECommerceApiDbContext context, IMapper mapper) {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProductListResponse> Handle(GetManyProductsQuery query, CancellationToken cancellationToken) {
        IQueryable<Product> source = _context.ProductTable
            .AsNoTracking()
            .Include(p => p.CategoryRisuto);

        if (!string.IsNullOrWhiteSpace(query.ProductName)) {
            var nameFiltered = source.Where(p => p.Name.Contains(query.ProductName));
            var descriptionFiltered = source.Where(p => p.Description.Contains(query.ProductName));

            source = nameFiltered.Union(descriptionFiltered);
        }

        if (!string.IsNullOrWhiteSpace(query.CategoryName))
            source = source.Where(p => p.CategoryRisuto.Any(c => c.Name.Contains(query.CategoryName)));

        if (!source.Any())
            throw new NotFoundException("No Product found");

        var limit = Math.Min(query.Limit, 100);
            
        var arranged = await source
            .OrderBy(p => p.Id)
            .Skip((query.Page - 1) * limit)
            .Take(limit)
            .ToListAsync(cancellationToken);

        var result = _mapper.Map<IEnumerable<ReadProductDto>>(arranged);

        return new ProductListResponse {Items = result};
    }
}


