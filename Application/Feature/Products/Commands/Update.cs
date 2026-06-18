using MediatR;
using ECommerceAPI.Infrastructure;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Domain;

namespace ECommerceAPI.Application;

public class UpdateProduct {
    public record UpdateProductCommand(UpdateProductDto UpdateProductDto) : IRequest<ReadProductDto>;

    public class Handler : IRequestHandler<UpdateProductCommand, ReadProductDto> {
        private readonly ECommerceApiDbContext _context;
        private readonly IMapper _mapper;
        public Handler(ECommerceApiDbContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ReadProductDto> Handle(UpdateProductCommand command, CancellationToken cancellationToken) {
            var theOne = await _context.ProductTable
                .Include(p => p.CategoryRisuto)
                .FirstOrDefaultAsync(p => p.Id == command.UpdateProductDto.Id, cancellationToken);

            if (theOne == null)
                throw new NotFoundException("Product not found");

            var categoryRisuto = await _context.CategoryTable
                .Where(c => command.UpdateProductDto.CategoryIdRisuto.Contains(c.Id))
                .ToListAsync(cancellationToken);

            if (categoryRisuto == null)
                throw new NotFoundException("Category not found");

            theOne.CategoryRisuto.Clear();
            _mapper.Map(command.UpdateProductDto, theOne);
            theOne.CategoryRisuto = categoryRisuto;
            
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ReadProductDto>(theOne);
        }
    }

}
