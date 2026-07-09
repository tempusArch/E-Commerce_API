using MediatR;

namespace ECommerceAPI.Application;

public record DeleteCategoryCommand(int Id) : IRequest<Unit>;
