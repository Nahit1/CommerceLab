using CommerceLab.Api.Common.Results;
using MediatR;

namespace CommerceLab.Api.Features.Orders.CreateOrder;

public sealed record Command(
    Guid CartId
) : IRequest<Result<Response>>;