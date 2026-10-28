namespace CommerceLab.Api.Domain;

public class Product
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}