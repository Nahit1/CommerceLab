using CommerceLab.Api.Common.Results;
using CommerceLab.Api.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CommerceLab.Api.Features.Catalog.GetProducts;

public sealed class Handler(AppDbContext db)
    : IRequestHandler<Query, Result<List<Response>>>
{
    public async Task<Result<List<Response>>> Handle(
        Query request,
        CancellationToken ct)
    {
        var products = await db.Products
            .AsNoTracking()
            .Join(
                db.Stocks,
                product => product.Id,
                stock => stock.ProductId,
                (product, stock) => new Response(
                    product.Id,
                    product.Name,
                    product.Price,
                    stock.Quantity
                )
            )
            .ToListAsync(ct);
        
        return Result<List<Response>>.Success(products);
    }
}