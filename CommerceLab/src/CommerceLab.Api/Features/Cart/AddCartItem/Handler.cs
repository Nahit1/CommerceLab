using CommerceLab.Api.Common.Results;
using CommerceLab.Api.Domain;
using CommerceLab.Api.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommerceLab.Api.Features.Cart.AddCartItem;

public sealed class Handler(AppDbContext db)
    : IRequestHandler<Command, Result<Response>>
{
    public async Task<Result<Response>> Handle(
        Command request,
        CancellationToken ct)
    {
        if (request.Quantity <= 0)
            return Result<Response>.Failure(
                Errors.InvalidQuantity);

        
        var productExists = await db.Products
            .AnyAsync(x => x.Id == request.ProductId, ct);

        if (!productExists)
            return Result<Response>.Failure(
                Errors.ProductNotFound);

        Domain.Cart? cart;

        if (request.CartId.HasValue)
        {
            cart = await db.Carts
                .Include(x => x.Items)
                .FirstOrDefaultAsync(
                    x => x.Id == request.CartId.Value, ct);

            if (cart is null)
                return Result<Response>.Failure(
                    Errors.CartNotFound);
        }
        else
        {
            cart = new Domain.Cart();
            db.Carts.Add(cart);
        }

        var item = cart.Items.FirstOrDefault(
            x => x.ProductId == request.ProductId);

        if (item is null)
        {
            item = new CartItem
            {
                CartId = cart.Id,
                ProductId = request.ProductId,
                Quantity = request.Quantity
            };

            cart.Items.Add(item);
        }
        else
        {
            item.Quantity += request.Quantity;
        }

        await db.SaveChangesAsync(ct);

        return Result<Response>.Success(new Response(
            cart.Id,
            item.ProductId,
            item.Quantity
        ));
    }
}