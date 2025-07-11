namespace MiniPay.Api.Models;

public class PaymentProvider
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string ApiUrl { get; set; }
    public bool IsActive { get; set; }
    public string SupportedCurrency { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
