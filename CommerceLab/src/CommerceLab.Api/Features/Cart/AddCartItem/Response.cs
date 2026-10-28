namespace CommerceLab.Api.Features.Cart.AddCartItem;

public sealed record Response(
    Guid CartId,
    Guid ProductId,
    int Quantity
);