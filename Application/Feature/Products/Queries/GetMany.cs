using MediatR;
using AutoMapper;
using ECommerceAPI.Application;
using ECommerceAPI.Infrastructure;
using ECommerceAPI.Domain;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Application;

public class GetManyProducts {
    public record GetManyProductsQuery(string? CategoryName, string? ProductName, int Page, int Limit) : IRequest<ProductListResponse>;

    public class Handler : IRequestHandler<GetManyProductsQuery, ProductListResponse> {
        private readonly ECommerceApiDbContext _context;
        private readonly IMapper _mapper;
        public Handler(ECommerceApiDbContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProductListResponse> Handle(GetManyProductsQuery query, CancellationToken cancellationToken) {
            IQueryable<Product> source = _context.ProductTable.AsNoTracking().Include(p => p.CategoryRisuto);

            if (!string.IsNullOrWhiteSpace(query.CategoryName))
                source = source.Where(p => p.CategoryRisuto.Any(c => c.Name.Contains(query.CategoryName)));

            if (!string.IsNullOrWhiteSpace(query.ProductName)) {
                var nameFiltered = source.Where(p => p.Name.Contains(query.ProductName));
                var descriptionFiltered = source.Where(p => p.Description.Contains(query.ProductName));

                source = nameFiltered.Union(descriptionFiltered);
            }

            if (source.Count() == 0)
                throw new NotFoundException("No Product found");

            if (query.Limit > 100) 
                query = query with {Limit = 100};
            
            var arranged = await source
                .OrderBy(p => p.Id)
                .Skip((query.Page - 1) * query.Limit)
                .Take(query.Limit)
                .ToListAsync(cancellationToken);

            var result = _mapper.Map<IEnumerable<ReadProductDto>>(arranged);

            return new ProductListResponse {Items = result};
        }
    }

}
