namespace CommerceLab.Api.Domain;

public class OrderItem
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }

    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}