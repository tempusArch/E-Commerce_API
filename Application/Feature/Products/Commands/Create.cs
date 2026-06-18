using AutoMapper;
using ECommerceAPI.Domain;
using ECommerceAPI.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Application;

public class CreateProduct {

    public record CreateProductCommand(CreateProductDto CreateProductDto) : IRequest<ReadProductDto>;

    public class Handler : IRequestHandler<CreateProductCommand, ReadProductDto> {
        private readonly ECommerceApiDbContext _context;
        private readonly IMapper _mapper;
        public Handler(ECommerceApiDbContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ReadProductDto> Handle(CreateProductCommand command, CancellationToken cancellationToken) {
            var isExisted = await _context.ProductTable.AnyAsync(c => c.Name == command.CreateProductDto.Name, cancellationToken);

            if (isExisted)
                throw new InvalidOperationException("Product has already existed");
            
            var categoryRisuto = await _context.CategoryTable
                .Where(c => command.CreateProductDto.CategoryIdRisuto.Contains(c.Id))
                .ToListAsync(cancellationToken);

            if (categoryRisuto == null)
                throw new NotFoundException("Category not found");

            var newProduct = _mapper.Map<Product>(command.CreateProductDto);
            newProduct.CategoryRisuto = categoryRisuto;

            _context.ProductTable.Add(newProduct);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ReadProductDto>(newProduct);
        }
    }
}    