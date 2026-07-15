using MediatR;
using ECommerceAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Application;

public class DeleteCartItemHandler : IRequestHandler<DeleteCartItemCommand, Unit> {
    private readonly ECommerceApiDbContext _context;
    public DeleteCartItemHandler(ECommerceApiDbContext context) {
        _context = context;         
    }

    public async Task<Unit> Handle(DeleteCartItemCommand command, CancellationToken cancellationToken) {
        var theOne = await _context.CartItemTable
            .FirstOrDefaultAsync(x => 
                x.ProductId == command.ProductId && 
                x.UserId  ==command.UserId,
                cancellationToken
            );

        if (theOne == null)
            throw new NotFoundException("Cart item not found");

        _context.CartItemTable.Remove(theOne);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}


