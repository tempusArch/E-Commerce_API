using MediatR;
using AutoMapper;
using ECommerceAPI.Application;
using ECommerceAPI.Infrastructure;
using ECommerceAPI.Domain;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Application;

public class GetManyCategories {
    public record GetManyCategoriesQuery(string? CategoryName, string? ProductName, int Page, int Limit) : IRequest<CategoryListResponse>;

    public class Handler : IRequestHandler<GetManyCategoriesQuery, CategoryListResponse> {
        private readonly ECommerceApiDbContext _context;
        private readonly IMapper _mapper;
        public Handler(ECommerceApiDbContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CategoryListResponse> Handle(GetManyCategoriesQuery query, CancellationToken cancellationToken) {
            IQueryable<Category> source = _context.CategoryTable.AsNoTracking().Include(p => p.ProductRisuto);

            if (!string.IsNullOrWhiteSpace(query.ProductName))
                source = source.Where(p => p.ProductRisuto.Any(c => c.Name.Contains(query.ProductName)));

            if (!string.IsNullOrWhiteSpace(query.CategoryName)) 
                source = source.Where(p => p.Name.Contains(query.CategoryName));

            if (source.Count() == 0)
                throw new NotFoundException("No Category found");
                 
            if (query.Limit > 100) 
                query = query with {Limit = 100};
            
            var arranged = await source
                .OrderBy(p => p.Id)
                .Skip((query.Page - 1) * query.Limit)
                .Take(query.Limit)
                .ToListAsync(cancellationToken);

            var result = _mapper.Map<IEnumerable<ReadCategoryDto>>(arranged);

            return new CategoryListResponse {Items = result};
        }
    }

}
