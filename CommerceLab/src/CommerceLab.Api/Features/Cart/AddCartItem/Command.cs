using CommerceLab.Api.Common.Results;
using MediatR;

namespace CommerceLab.Api.Features.Cart.AddCartItem;

public sealed record Command(
    Guid? CartId,
    Guid ProductId,
    int Quantity
) : IRequest<Result<Response>>;