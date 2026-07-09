using MediatR;
using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Infrastructure;

namespace ECommerceAPI.Application;

public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, Unit> {
    private readonly ECommerceApiDbContext _context;
    public DeleteCategoryHandler(ECommerceApiDbContext context) {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteCategoryCommand command, CancellationToken cancellationToken) {
        var theOne = await _context.CategoryTable
            .FirstOrDefaultAsync(c => c.Id == command.Id, cancellationToken);

        if (theOne == null)
            throw new NotFoundException("Category not found");

        _context.CategoryTable.Remove(theOne);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}