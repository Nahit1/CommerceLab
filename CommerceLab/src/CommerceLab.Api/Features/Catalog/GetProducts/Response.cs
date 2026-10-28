namespace CommerceLab.Api.Features.Catalog.GetProducts;

public record Response(
    Guid Id,
    string Name,
    decimal Price,
    int Stock
);