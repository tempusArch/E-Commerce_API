using MediatR;

namespace ECommerceAPI.Application;

public record GetOneCategoryQuery(int Id) : IRequest<ReadCategoryDto>;