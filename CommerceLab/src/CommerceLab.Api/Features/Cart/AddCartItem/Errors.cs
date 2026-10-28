using CommerceLab.Api.Common.Results;

namespace CommerceLab.Api.Features.Cart.AddCartItem;

public static class Errors
{
    public static readonly Error InvalidQuantity =
        new("Cart.InvalidQuantity", "Quantity must be greater than zero.");

    public static readonly Error CartNotFound =
        new("Cart.NotFound", "Cart not found.");

    public static readonly Error ProductNotFound =
        new("Product.NotFound", "Product not found.");
}