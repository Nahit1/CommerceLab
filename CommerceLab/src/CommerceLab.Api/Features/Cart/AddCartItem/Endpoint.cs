using MediatR;

namespace CommerceLab.Api.Features.Cart.AddCartItem;

public static class Endpoint
{
    public static IEndpointRouteBuilder MapAddCartItem(
        this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/carts/items",
                async (
                    Request request,
                    ISender sender,
                    CancellationToken ct) =>
                {
                    var result = await sender.Send(
                        new Command(
                            request.CartId,
                            request.ProductId,
                            request.Quantity),
                        ct);

                    return result.Error?.Code switch
                    {
                        "Cart.InvalidQuantity" =>
                            Results.BadRequest(result.Error),

                        "Cart.NotFound" or "Product.NotFound" =>
                            Results.NotFound(result.Error),

                        null => Results.Ok(result.Value),

                        _ => Results.Problem()
                    };
                })
            .WithName("AddCartItem")
            .WithTags("Cart");

        return app;
    }
}

public sealed record Request(
    Guid? CartId,
    Guid ProductId,
    int Quantity
);