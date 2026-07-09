using MediatR;
using ECommerceAPI.Domain;

namespace ECommerceAPI.Application;

public record UpdateProductCommand(UpdateProductDto UpdateProductDto) : IRequest<Product>;