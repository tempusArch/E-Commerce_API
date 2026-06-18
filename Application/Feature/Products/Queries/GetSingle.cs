using MediatR;
using AutoMapper;
using ECommerceAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Application;

public class GetSingleProduct {
    public record GetSingleProductQuery(int Id) : IRequest<ReadProductDto>;

    public class Handler : IRequestHandler<GetSingleProductQuery, ReadProductDto> {
        private readonly ECommerceApiDbContext _context;
        private readonly IMapper _mapper;
        public Handler(ECommerceApiDbContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ReadProductDto> Handle(GetSingleProductQuery query, CancellationToken cancellationToken) {
            var theOne = await _context.ProductTable
                .AsNoTracking()
                .Include(p => p.CategoryRisuto)
                .FirstOrDefaultAsync(p => p.Id == query.Id, cancellationToken);

            if (theOne == null)
                throw new NotFoundException("Product not found");

            return _mapper.Map<ReadProductDto>(theOne);
        }
    }

}
