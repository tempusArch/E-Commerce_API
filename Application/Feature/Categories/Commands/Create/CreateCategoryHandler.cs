using AutoMapper;
using ECommerceAPI.Domain;
using ECommerceAPI.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Application;

public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, Category> {
    private readonly ECommerceApiDbContext _context;
    private readonly IMapper _mapper;
    public CreateCategoryHandler(ECommerceApiDbContext context, IMapper mapper) {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Category> Handle(CreateCategoryCommand command, CancellationToken cancellationToken) {
        var isExisted = await _context.CategoryTable
            .AnyAsync(c => c.Name == command.Name, cancellationToken);

        if (isExisted)
            throw new InvalidOperationException("Category has already existed");

        var newOne = new Category {
            Name = command.Name
        };

        _context.CategoryTable.Add(newOne);
        await _context.SaveChangesAsync(cancellationToken);

        return newOne;
    }
}
