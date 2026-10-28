using MediatR;

namespace CommerceLab.Api.Features.Catalog.GetProducts;

public static class Endpoint
{
    public static IEndpointRouteBuilder MapGetProducts(
        this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/products",
                async (ISender sender, CancellationToken ct) =>
                {
                    var result = await sender.Send(new Query(), ct);

                    return Results.Ok(result.Value);
                })
            .WithName("GetProducts")
            .WithTags("Catalog");

        return app;
    }
}