using CommerceLab.Api.Features.Cart.AddCartItem;
using CommerceLab.Api.Features.Catalog.GetProducts;
using CommerceLab.Api.Features.Orders.CreateOrder;
using CommerceLab.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration
                           .GetConnectionString("Default")
                       ?? throw new InvalidOperationException(
                           "Database connection string is missing.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(
        typeof(Program).Assembly);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await using var scope = app.Services.CreateAsyncScope();

    var db = scope.ServiceProvider
        .GetRequiredService<AppDbContext>();

    await SeedData.InitializeAsync(db);
}

app.MapGetProducts();
app.MapCreateOrder();
app.MapAddCartItem();

app.Run();

