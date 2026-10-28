namespace CommerceLab.Api.Features.Orders.CreateOrder;

public sealed record Response(
    Guid OrderId,
    decimal TotalAmount,
    string Status
);