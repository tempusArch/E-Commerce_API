using AutoMapper;
using ECommerceAPI.Domain;
using ECommerceAPI.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Application;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, Product> {
    private readonly ECommerceApiDbContext _context;
    private readonly IMapper _mapper;
    public CreateProductHandler(ECommerceApiDbContext context, IMapper mapper) {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Product> Handle(CreateProductCommand command, CancellationToken cancellationToken) {
        var isExisted = await _context.ProductTable
            .AnyAsync(c => c.Name == command.CreateProductDto.Name, cancellationToken);

        if (isExisted)
            throw new InvalidOperationException("Product name already existed");
            
        var categoryRisuto = await _context.CategoryTable
            .Where(c => command.CreateProductDto.CategoryIdRisuto.Contains(c.Id))
            .ToListAsync(cancellationToken);

        if (!categoryRisuto.Any())
            throw new NotFoundException("Category not found");

        var newOne = _mapper.Map<Product>(command.CreateProductDto);
        newOne.CategoryRisuto = categoryRisuto;

        _context.ProductTable.Add(newOne);
        await _context.SaveChangesAsync(cancellationToken);

        return newOne;
    }
} 