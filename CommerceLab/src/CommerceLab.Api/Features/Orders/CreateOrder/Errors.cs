using CommerceLab.Api.Common.Results;

namespace CommerceLab.Api.Features.Orders.CreateOrder;

public static class Errors
{
    public static readonly Error CartNotFound =
        new("Cart.NotFound", "Cart not found.");

    public static readonly Error EmptyCart =
        new("Cart.Empty", "Cart is empty.");

    public static readonly Error ProductNotFound =
        new("Product.NotFound", "Product not found.");

    
    public static readonly Error InvalidQuantity =
        new("Order.InvalidQuantity",
            "Quantity must be greater than zero.");
}