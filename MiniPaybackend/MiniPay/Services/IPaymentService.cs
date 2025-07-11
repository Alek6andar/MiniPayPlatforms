using MiniPay.Api.Models;

namespace MiniPay.Api.Services;

public interface IPaymentService
{
    Task<PaymentResponse> ProcessPaymentAsync(PaymentRequest request);
}
