using MiniPay.Api.Models;
using System.Net.Http.Json;

namespace MiniPay.Api.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentProviderRepository _repository;
    private readonly HttpClient _httpClient;
    private readonly ILogger<PaymentService> _logger;

    public PaymentService(
        IPaymentProviderRepository repository,
        HttpClient httpClient,
        ILogger<PaymentService> logger)
    {
        _repository = repository;
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<PaymentResponse> ProcessPaymentAsync(PaymentRequest request)
    {
        var provider = await _repository.GetByIdAsync(request.ProviderId);
        if (provider == null)
        {
            _logger.LogWarning("Provider not found: {ProviderId}", request.ProviderId);
            throw new InvalidOperationException("Provider not found");
        }

        if (!provider.IsActive)
        {
            _logger.LogWarning("Provider is inactive: {ProviderName}", provider.Name);
            throw new InvalidOperationException("Provider is inactive");
        }

        if (provider.SupportedCurrency != request.Currency)
        {
            _logger.LogWarning("Currency mismatch. Provider: {SupportedCurrency}, Request: {Currency}",
                provider.SupportedCurrency, request.Currency);
            throw new InvalidOperationException($"Provider only supports {provider.SupportedCurrency}");
        }

        try
        {
            _logger.LogInformation("Processing payment with {ProviderName}", provider.Name);

            var response = await _httpClient.PostAsJsonAsync(provider.ApiUrl, new
            {
                request.Amount,
                request.Currency,
                request.Description,
                request.ReferenceId
            });

            response.EnsureSuccessStatusCode();

            var paymentResponse = await response.Content.ReadFromJsonAsync<PaymentResponse>();

            if (paymentResponse == null)
            {
                throw new Exception("Invalid payment response");
            }

            _logger.LogInformation("Payment processed. Status: {Status}", paymentResponse.Status);

            return paymentResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Payment processing failed");
            return new PaymentResponse
            {
                Status = "Failed",
                Message = ex.Message,
                Timestamp = DateTime.UtcNow,
                TransactionId = "",
                ReferenceId = request.ReferenceId
            };
        }
    }
}