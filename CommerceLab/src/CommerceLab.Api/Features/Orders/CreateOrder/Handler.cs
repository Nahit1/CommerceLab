
using CommerceLab.Api.Common.Results;
using CommerceLab.Api.Domain;
using CommerceLab.Api.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommerceLab.Api.Features.Orders.CreateOrder;

public sealed class Handler(AppDbContext db)
    : IRequestHandler<Command, Result<Response>>
{
    public async Task<Result<Response>> Handle(
        Command request,
        CancellationToken ct)
    {
        // 1. Sepeti getir
        var cart = await db.Carts
            .Include(x => x.Items)
            .FirstOrDefaultAsync(
                x => x.Id == request.CartId, ct);

        if (cart is null)
            return Result<Response>.Failure(
                Errors.CartNotFound);

        if (cart.Items.Count == 0)
            return Result<Response>.Failure(
                Errors.EmptyCart);

        // 2. Ürün bilgilerini getir
        var productIds = cart.Items
            .Select(x => x.ProductId)
            .ToArray();

        var products = await db.Products
            .AsNoTracking()
            .Where(x => productIds.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, ct);

        // 3. Siparişi oluştur
        var order = new Order
        {
            Status = OrderStatus.PendingPayment
        };

        foreach (var item in cart.Items)
        {
            if (item.Quantity <= 0)
                return Result<Response>.Failure(
                    Errors.InvalidQuantity);

            if (!products.TryGetValue(
                    item.ProductId, out var product))
                return Result<Response>.Failure(
                    Errors.ProductNotFound);

            order.Items.Add(new OrderItem
            {
                ProductId = product.Id,
                Quantity = item.Quantity,
                UnitPrice = product.Price
            });

            order.TotalAmount +=
                product.Price * item.Quantity;
        }

        // 4. Payment Processing
        // TODO: Payment Provider entegrasyonu
        // V1: Ödeme başarılı simüle ediliyor
        var paymentSucceeded = true;

        var payment = new Payment
        {
            OrderId = order.Id,
            Amount = order.TotalAmount,
            Status = paymentSucceeded
                ? PaymentStatus.Succeeded
                : PaymentStatus.Failed
        };

        db.Orders.Add(order);
        db.Payments.Add(payment);

        if (paymentSucceeded)
        {
            // 5. Ödeme başarılıysa stok düş
            var stocks = await db.Stocks
                .Where(x => productIds.Contains(x.ProductId))
                .ToDictionaryAsync(x => x.ProductId, ct);

            foreach (var item in cart.Items)
            {
                var stock = stocks[item.ProductId];
                stock.Quantity -= item.Quantity;
            }

            order.Status = OrderStatus.Confirmed;

            // Satın alma tamamlandı: Sepeti temizle
            db.Carts.Remove(cart);
        }
        else
        {
            // Ödeme başarısızsa stok değişmez
            // Sepet korunur
            order.Status = OrderStatus.PaymentFailed;
        }

        // 6. Değişiklikleri kaydet
        await db.SaveChangesAsync(ct);

        return Result<Response>.Success(
            new Response(
                order.Id,
                order.TotalAmount,
                order.Status.ToString()
            )
        );
    }
}
