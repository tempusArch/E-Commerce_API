using MediatR;
using ECommerceAPI.Domain;

namespace ECommerceAPI.Application;

public record UpdateProductCommand(int ProductId, UpdateProductDto UpdateProductDto) : IRequest<Product>;