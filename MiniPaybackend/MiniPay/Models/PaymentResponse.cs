namespace MiniPay.Api.Models
{
    public class PaymentResponse
    {
        public string Status { get; set; }
        public string TransactionId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Message { get; set; }
        public string ReferenceId { get; set; }
    }
}
