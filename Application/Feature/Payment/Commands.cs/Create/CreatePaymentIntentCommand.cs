using MediatR;

namespace ECommerceAPI.Application;

public record CreatePaymentIntentCommand(int UserId, int OrderId) : IRequest<string>;