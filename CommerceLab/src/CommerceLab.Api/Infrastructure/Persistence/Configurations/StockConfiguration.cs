using CommerceLab.Api.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommerceLab.Api.Infrastructure.Persistence.Configurations;

public class StockConfiguration
    : IEntityTypeConfiguration<Stock>
{
    public void Configure(EntityTypeBuilder<Stock> builder)
    {
        builder.ToTable("Stocks");

        builder.HasKey(x => x.ProductId);

        builder.Property(x => x.Quantity)
            .IsRequired();

        builder.HasOne<Product>()
            .WithOne()
            .HasForeignKey<Stock>(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}