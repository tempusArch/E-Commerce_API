using MediatR;
using ECommerceAPI.Infrastructure;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Domain;

namespace ECommerceAPI.Application;

public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, Category> {
    private readonly ECommerceApiDbContext _context;
    private readonly IMapper _mapper;
    public UpdateCategoryHandler(ECommerceApiDbContext context, IMapper mapper) {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Category> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken) {
        var theOne = await _context.CategoryTable
            .FirstOrDefaultAsync(c => c.Id == command.CategoryId);

        if (theOne == null)
            throw new NotFoundException("Category not found");

        var isExisted = await _context.CategoryTable.AnyAsync(c => c.Name == command.CategoryName, cancellationToken);

        if (isExisted)
            throw new InvalidOperationException("Category has already existed");

        theOne.Name = command.CategoryName;
        await _context.SaveChangesAsync(cancellationToken);

        return theOne;
    }
}
