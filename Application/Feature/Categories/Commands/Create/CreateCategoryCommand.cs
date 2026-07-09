using MediatR;
using ECommerceAPI.Domain;

namespace ECommerceAPI.Application;

public record CreateCategoryCommand(string Name) : IRequest<Category>;
