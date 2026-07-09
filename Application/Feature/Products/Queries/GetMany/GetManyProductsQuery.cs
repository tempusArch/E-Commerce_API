using MediatR;

namespace ECommerceAPI.Application;

public record GetManyProductsQuery(string? CategoryName, string? ProductName, int Page, int Limit) : IRequest<ProductListResponse>;