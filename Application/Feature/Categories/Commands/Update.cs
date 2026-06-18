using MediatR;
using ECommerceAPI.Infrastructure;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Domain;

namespace ECommerceAPI.Application;

public class UpdateCategory {
    public record UpdateCategoryCommand(int CategoryId, string CategoryName) : IRequest<Category>;

    public class Handler : IRequestHandler<UpdateCategoryCommand, Category> {
        private readonly ECommerceApiDbContext _context;
        private readonly IMapper _mapper;
        public Handler(ECommerceApiDbContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Category> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken) {
            var theOne = await _context.CategoryTable
                .FirstOrDefaultAsync(c => c.Id == command.CategoryId);

            if (theOne == null)
                throw new NotFoundException("Category not found");

            theOne.Name = command.CategoryName;
            await _context.SaveChangesAsync(cancellationToken);

            return theOne;
        }
    }

}
