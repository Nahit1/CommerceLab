namespace CommerceLab.Api.Domain;

public class Order
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public decimal TotalAmount { get; set; }

    public OrderStatus Status { get; set; }
        = OrderStatus.PendingPayment;

    public List<OrderItem> Items { get; set; } = [];
}

public enum OrderStatus
{
    PendingPayment,
    Confirmed,
    PaymentFailed
}