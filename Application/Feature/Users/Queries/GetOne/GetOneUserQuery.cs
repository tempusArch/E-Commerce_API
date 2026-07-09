using MediatR;
using ECommerceAPI.Domain;

namespace ECommerceAPI.Application;

public record GetOneUserQuery(int Id) : IRequest<User>;