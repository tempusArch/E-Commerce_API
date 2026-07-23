using MediatR;
using AutoMapper;
using ECommerceAPI.Infrastructure;
using ECommerceAPI.Domain;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Application;

public class GetManyCategoriesHandler : IRequestHandler<GetManyCategoriesQuery, CategoryListResponse> {
    private readonly ECommerceApiDbContext _context;
    private readonly IMapper _mapper;
    public GetManyCategoriesHandler(ECommerceApiDbContext context, IMapper mapper) {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CategoryListResponse> Handle(GetManyCategoriesQuery query, CancellationToken cancellationToken) {
        IQueryable<Category> source = _context.CategoryTable.AsNoTracking();
            
        if (!string.IsNullOrWhiteSpace(query.CategoryName)) 
            source = source.Where(p => p.Name.Contains(query.CategoryName));

        if (!string.IsNullOrWhiteSpace(query.ProductName))
            source = source.Where(p => p.ProductRisuto.Any(c => c.Name.Contains(query.ProductName)));
        
        var limit = Math.Min(query.Limit, 100);

        var result = await source
            .Select(x => new ReadCategoryDto {
                CategoryId = x.Id,
                CategoryName = x.Name,
                ProductRisuto = x.ProductRisuto
                    .Select(z => new ReadProductDto {
                        ProductId = z.Id,
                        ProductName = z.Name,
                        ProductDescription = z.Description,
                        Price = z.Price
                    })
                    .ToList()
            })
            .OrderBy(p => p.CategoryName)
            .Skip((query.Page - 1) * limit)
            .Take(limit)
            .ToListAsync(cancellationToken);

        return new CategoryListResponse {Items = result};
    }
}
