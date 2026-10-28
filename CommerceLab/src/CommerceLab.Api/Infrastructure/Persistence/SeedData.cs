using CommerceLab.Api.Domain;
using CommerceLab.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace CommerceLab.Api.Infrastructure.Persistence;

public static class SeedData
{
    public static async Task InitializeAsync(AppDbContext db)
    {
        const string productName = "MacBook Pro 14";

        var product = await db.Products
            .FirstOrDefaultAsync(x => x.Name == productName);

        if (product is null)
        {
            product = new Product
            {
                Id = Guid.CreateVersion7(),
                Name = productName,
                Price = 75000m
            };

            db.Products.Add(product);
        }

        var stockExists = await db.Stocks
            .AnyAsync(x => x.ProductId == product.Id);

        if (!stockExists)
        {
            db.Stocks.Add(new Stock
            {
                ProductId = product.Id,
                Quantity = 10
            });
        }

        await db.SaveChangesAsync();
    }
}