using MediatR;
using AutoMapper;
using ECommerceAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Application;

public class GetOneCategory {
    public record GetOneCategoryQuery(int Id) : IRequest<ReadCategoryDto>;

    public class Handler : IRequestHandler<GetOneCategoryQuery, ReadCategoryDto> {
        private readonly ECommerceApiDbContext _context;
        private readonly IMapper _mapper;
        public Handler(ECommerceApiDbContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ReadCategoryDto> Handle(GetOneCategoryQuery query, CancellationToken cancellationToken) {
            var theOne = await _context.CategoryTable
                .AsNoTracking()
                .Include(p => p.ProductRisuto)
                .FirstOrDefaultAsync(c => c.Id == query.Id, cancellationToken);

            if (theOne == null)
                throw new NotFoundException("Category not found");

            return _mapper.Map<ReadCategoryDto>(theOne);
        }
    }

}
