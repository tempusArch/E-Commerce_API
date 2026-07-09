using MediatR;

namespace ECommerceAPI.Application;

public record GetOneProductQuery(int Id) : IRequest<ReadProductDto>;