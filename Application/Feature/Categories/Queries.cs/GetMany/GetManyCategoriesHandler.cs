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
        IQueryable<Category> source = _context.CategoryTable
            .AsNoTracking()
            .Include(p => p.ProductRisuto);

            
        if (!string.IsNullOrWhiteSpace(query.CategoryName)) 
            source = source.Where(p => p.Name.Contains(query.CategoryName));

        if (!string.IsNullOrWhiteSpace(query.ProductName))
            source = source.Where(p => p.ProductRisuto.Any(c => c.Name.Contains(query.ProductName)));

        if (!source.Any())
            throw new NotFoundException("No Category Found");
                 
        var limit = Math.Min(query.Limit, 100);
            
        var arranged = await source
            .OrderBy(p => p.Id)
            .Skip((query.Page - 1) * limit)
            .Take(limit)
            .ToListAsync(cancellationToken);

        var result = _mapper.Map<IEnumerable<ReadCategoryDto>>(arranged);

        return new CategoryListResponse {Items = result};
    }
}
