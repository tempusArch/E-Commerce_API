using MediatR;
using ECommerceAPI.Domain;

namespace ECommerceAPI.Application;

public record CreateProductCommand(CreateProductDto CreateProductDto) : IRequest<Product>;