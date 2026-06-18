using AutoMapper;
using ECommerceAPI.Domain;
using ECommerceAPI.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Application;

public class CreateCategory {

    public record CreateCategoryCommand(string CategoryName) : IRequest<Category>;

    public class Handler : IRequestHandler<CreateCategoryCommand, Category> {
        private readonly ECommerceApiDbContext _context;
        private readonly IMapper _mapper;
        public Handler(ECommerceApiDbContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Category> Handle(CreateCategoryCommand command, CancellationToken cancellationToken) {
            var isExisted = await _context.CategoryTable.AnyAsync(c => c.Name == command.CategoryName, cancellationToken);

            if (isExisted)
                throw new InvalidOperationException("Category has already existed");

            var newOne = new Category {
                Name = command.CategoryName
            };

            _context.CategoryTable.Add(newOne);
            await _context.SaveChangesAsync(cancellationToken);

            return newOne;
        }
    }
}    