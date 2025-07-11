namespace MiniPay.Api.Models;

public class PaymentRequest
{
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string? Description { get; set; }
    public Guid ProviderId { get; set; }
    public string ReferenceId { get; set; } = $"ORDER-{Guid.NewGuid()}";
}
