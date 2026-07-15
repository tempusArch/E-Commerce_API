using MediatR;
using ECommerceAPI.Infrastructure;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Domain;

namespace ECommerceAPI.Application;

public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, Product> {
    private readonly ECommerceApiDbContext _context;
    private readonly IMapper _mapper;
    public UpdateProductHandler(ECommerceApiDbContext context, IMapper mapper) {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Product> Handle(UpdateProductCommand command, CancellationToken cancellationToken) {
        var theOne = await _context.ProductTable
            .Include(p => p.CategoryRisuto)
            .FirstOrDefaultAsync(p => p.Id == command.ProductId, cancellationToken);

        if (theOne == null)
            throw new NotFoundException("Product not found");

        var categoryRisuto = await _context.CategoryTable
            .Where(c => command.UpdateProductDto.CategoryIdRisuto.Contains(c.Id))
            .ToListAsync(cancellationToken);

        if (!categoryRisuto.Any())
            throw new NotFoundException("Category not found");

        theOne.CategoryRisuto.Clear();
        _mapper.Map(command.UpdateProductDto, theOne);
        theOne.CategoryRisuto = categoryRisuto;
            
        await _context.SaveChangesAsync(cancellationToken);

        return theOne;
    }
}