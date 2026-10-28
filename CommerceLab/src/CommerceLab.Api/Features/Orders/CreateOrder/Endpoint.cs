using MediatR;

namespace CommerceLab.Api.Features.Orders.CreateOrder;

public static class Endpoint
{
    public static IEndpointRouteBuilder MapCreateOrder(
        this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/orders",
                async (
                    Request request,
                    ISender sender,
                    CancellationToken ct) =>
                {
                    var result = await sender.Send(
                        new Command(request.CartId), ct);

                    return result.Error?.Code switch
                    {
                        "Cart.NotFound" =>
                            Results.NotFound(result.Error),

                        "Cart.Empty" =>
                            Results.BadRequest(result.Error),

                        "Product.NotFound" =>
                            Results.NotFound(result.Error),

                        "Stock.Insufficient" =>
                            Results.Conflict(result.Error),

                        null => Results.Json(
                            result.Value,
                            statusCode: StatusCodes.Status201Created),

                        _ => Results.Problem()
                    };
                })
            .WithName("CreateOrder")
            .WithTags("Orders");

        return app;
    }
}

public sealed record Request(Guid CartId);