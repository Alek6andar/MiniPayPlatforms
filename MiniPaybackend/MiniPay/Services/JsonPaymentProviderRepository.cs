using MiniPay.Api.Models;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace MiniPay.Api.Services;

public class JsonPaymentProviderRepository : IPaymentProviderRepository
{
    private const string JsonFilePath = "Data/PaymentProviders.json";
    private readonly List<PaymentProvider> _providers;
    private readonly ILogger<JsonPaymentProviderRepository> _logger;
    private readonly object _fileLock = new();

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = null, 
        WriteIndented = true,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };

    public JsonPaymentProviderRepository(ILogger<JsonPaymentProviderRepository> logger)
    {
        _logger = logger;
        _providers = LoadProviders();

        // Initialize with sample data if empty
        if (_providers.Count == 0)
        {
            _providers.AddRange(GetInitialProviders());
            _ = SaveProviders();
        }
    }

    public async Task<IEnumerable<PaymentProvider>> GetAllAsync() =>
        await Task.FromResult(_providers.AsEnumerable());

    public async Task<PaymentProvider?> GetByIdAsync(Guid id) =>
        await Task.FromResult(_providers.FirstOrDefault(p => p.Id == id));

    public async Task AddAsync(PaymentProvider provider)
    {
        provider.Id = Guid.NewGuid();
        provider.CreatedAt = DateTime.UtcNow;
        _providers.Add(provider);
        await SaveProviders();
    }

    public async Task UpdateAsync(PaymentProvider provider)
    {
        var index = _providers.FindIndex(p => p.Id == provider.Id);
        if (index >= 0)
        {
            provider.UpdatedAt = DateTime.UtcNow;
            _providers[index] = provider;
            await SaveProviders();
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var provider = _providers.FirstOrDefault(p => p.Id == id);
        if (provider != null)
        {
            _providers.Remove(provider);
            await SaveProviders();
        }
    }

    public async Task ToggleActiveStatusAsync(Guid id)
    {
        var provider = _providers.FirstOrDefault(p => p.Id == id);
        if (provider != null)
        {
            provider.IsActive = !provider.IsActive;
            provider.UpdatedAt = DateTime.UtcNow;
            await SaveProviders();
        }
    }

    private List<PaymentProvider> LoadProviders()
    {
        lock (_fileLock)
        {
            try
            {
                if (!File.Exists(JsonFilePath))
                {
                    return new List<PaymentProvider>();
                }

                using var stream = File.OpenRead(JsonFilePath);
                return JsonSerializer.Deserialize<List<PaymentProvider>>(stream, _jsonOptions)
                    ?? new List<PaymentProvider>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading providers from JSON file");
                return new List<PaymentProvider>();
            }
        }
    }

    private async Task SaveProviders()
    {
        await Task.Run(() =>
        {
            lock (_fileLock)
            {
                try
                {
                    using var stream = File.Create(JsonFilePath);
                    JsonSerializer.Serialize(stream, _providers.OrderBy(p => p.Name), _jsonOptions);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error saving providers to JSON file");
                }
            }
        });
    }

    private static IEnumerable<PaymentProvider> GetInitialProviders()
    {
        return new List<PaymentProvider>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Stripe",
                ApiUrl = "https://api.stripe.com/v1/payments",
                IsActive = true,
                SupportedCurrency = "USD",
                Description = "Stripe payment provider",
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "PayPal",
                ApiUrl = "https://api.paypal.com/v1/payments",
                IsActive = true,
                SupportedCurrency = "EUR",
                Description = "PayPal payment provider",
                CreatedAt = DateTime.UtcNow
            }
        };
    }
}
