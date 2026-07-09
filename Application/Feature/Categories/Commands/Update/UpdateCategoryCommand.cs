using MediatR;
using ECommerceAPI.Domain;

namespace ECommerceAPI.Application;

public record UpdateCategoryCommand(int CategoryId, string CategoryName) : IRequest<Category>;