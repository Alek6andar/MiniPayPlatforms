using MiniPay.Api.Models;

namespace MiniPay.Api.Services;

public interface IPaymentProviderRepository
{
    Task<IEnumerable<PaymentProvider>> GetAllAsync();
    Task<PaymentProvider?> GetByIdAsync(Guid id);
    Task AddAsync(PaymentProvider provider);
    Task UpdateAsync(PaymentProvider provider);
    Task DeleteAsync(Guid id);
    Task ToggleActiveStatusAsync(Guid id);
}