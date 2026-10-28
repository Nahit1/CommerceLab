namespace CommerceLab.Api.Domain;

public class Payment
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid OrderId { get; set; }

    public decimal Amount { get; set; }
    public PaymentStatus Status { get; set; }
        = PaymentStatus.Pending;
}

public enum PaymentStatus
{
    Pending,
    Succeeded,
    Failed
}