using Microsoft.AspNetCore.Mvc;

namespace MiniPay.Controllers
{
    [ApiController]
    [Route("mock")]
    public class MockPaymentController : ControllerBase
    {
        [HttpPost("paypal")]
        public IActionResult PayPalMock([FromBody] object request)
        {
            return Ok(new
            {
                status = "Success",
                transactionId = "TXPP" + Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper(),
                timestamp = DateTime.UtcNow,
                message = "Payment processed successfully",
                referenceId = "ORDER-12345"
            });
        }

        [HttpPost("stripe")]
        public IActionResult StripeMock([FromBody] object request)
        {
            return Ok(new
            {
                status = "Success",
                transactionId = "TXST" + Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper(),
                timestamp = DateTime.UtcNow,
                message = "Payment processed successfully",
                referenceId = "ORDER-12345"
            });
        }
    }
}

