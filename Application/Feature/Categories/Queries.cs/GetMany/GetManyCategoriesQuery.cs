using MediatR;

namespace ECommerceAPI.Application;

public record GetManyCategoriesQuery(string? CategoryName, string? ProductName, int Page, int Limit) : IRequest<CategoryListResponse>;