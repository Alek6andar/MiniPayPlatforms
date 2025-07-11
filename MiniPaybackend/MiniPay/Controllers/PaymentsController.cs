using Microsoft.AspNetCore.Mvc;
using MiniPay.Api.Models;
using MiniPay.Api.Services;

namespace MiniPay.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost]
    public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequest request)
    {
        var response = await _paymentService.ProcessPaymentAsync(request);
        return Ok(response);
    }
}