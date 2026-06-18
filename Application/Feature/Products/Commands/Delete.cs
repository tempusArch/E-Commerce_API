using MediatR;
using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Infrastructure;

namespace ECommerceAPI.Application;

public class DeleteProduct {
    public record DeleteProductCommand(int Id) : IRequest<Unit>;

    public class Handler : IRequestHandler<DeleteProductCommand, Unit> {
        private readonly ECommerceApiDbContext _context;
        public Handler(ECommerceApiDbContext context) {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteProductCommand command, CancellationToken cancellationToken) {
            var theOne = await _context.ProductTable
                .FirstOrDefaultAsync(p => p.Id == command.Id, cancellationToken);

            if (theOne == null)
                throw new NotFoundException("Product not found");

            _context.ProductTable.Remove(theOne);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}