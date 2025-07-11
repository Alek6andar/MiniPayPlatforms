using Microsoft.AspNetCore.Mvc;
using MiniPay.Api.Models;
using MiniPay.Api.Services;

namespace MiniPay.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentProvidersController : ControllerBase
{
    private readonly IPaymentProviderRepository _repository;
    private readonly ILogger<PaymentProvidersController> _logger;

    public PaymentProvidersController(
        IPaymentProviderRepository repository,
        ILogger<PaymentProvidersController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var providers = await _repository.GetAllAsync();
        return Ok(providers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var provider = await _repository.GetByIdAsync(id);
        if (provider == null) return NotFound();
        return Ok(provider);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PaymentProvider provider)
    {
        provider.Id = Guid.NewGuid();
        provider.CreatedAt = DateTime.UtcNow;
        await _repository.AddAsync(provider);
        return CreatedAtAction(nameof(GetById), new { id = provider.Id }, provider);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] PaymentProvider provider)
    {
        if (id != provider.Id) return BadRequest();
        provider.UpdatedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(provider);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _repository.DeleteAsync(id);
        return NoContent();
    }

    [HttpPatch("{id}/toggle-active")]
    public async Task<IActionResult> ToggleActive(Guid id)
    {
        await _repository.ToggleActiveStatusAsync(id);
        return NoContent();
    }
}
