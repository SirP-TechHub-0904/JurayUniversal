using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Website.Controllers
{
    /// <summary>
    /// PixelPay Webhook Controller.
    /// Register this URL in your PixelPay dashboard: Settings → Webhooks
    /// Endpoint: POST https://yourdomain.com/api/pixelpay-webhook
    ///
    /// PixelPay will POST to this endpoint whenever a payment event occurs.
    /// Event types: charge.success, charge.failed, refund.processed, transfer.success
    ///
    /// Payload contains: { "status": "PAID", "reference": "TX-...", "amount": 5000, ... }
    /// </summary>
    [ApiController]
    [Route("api/pixelpay-webhook")]
    [Microsoft.AspNetCore.Authorization.AllowAnonymous] // PixelPay servers POST here — must be publicly accessible
    public class PixelPayWebhookController : ControllerBase
    {
        private readonly DashboardDbContext _context;

        public PixelPayWebhookController(DashboardDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public async Task<IActionResult> HandleWebhook()
        {
            try
            {
                // Read raw body
                using var reader = new StreamReader(Request.Body);
                var rawBody = await reader.ReadToEndAsync();

                using var doc = JsonDocument.Parse(rawBody);
                var root = doc.RootElement;

                // Extract reference and status from PixelPay webhook payload
                string reference = null;
                string status = null;
                decimal amount = 0;

                if (root.TryGetProperty("reference", out var refEl)) reference = refEl.GetString();
                if (root.TryGetProperty("status", out var statusEl)) status = statusEl.GetString()?.ToUpperInvariant();
                if (root.TryGetProperty("amount", out var amountEl)) amountEl.TryGetDecimal(out amount);

                if (string.IsNullOrEmpty(reference))
                    return BadRequest(new { message = "No reference in payload." });

                // Find our transaction
                var tx = await _context.MisFinanceTransactions
                    .FirstOrDefaultAsync(t => t.TransactionReference == reference);

                if (tx == null)
                    return Ok(new { message = "Reference not found — ignored." });

                // Update status based on PixelPay payload
                if (status == "PAID" || status == "SUCCESS" || status == "00")
                {
                    if (tx.Status != PaymentStatus.Success)
                    {
                        tx.Status = PaymentStatus.Success;
                        tx.PaidAt = DateTime.UtcNow;
                        await _context.SaveChangesAsync();
                    }
                }
                else if (status == "FAILED" || status == "DECLINED" || status == "05")
                {
                    if (tx.Status == PaymentStatus.Pending)
                    {
                        tx.Status = PaymentStatus.Failed;
                        await _context.SaveChangesAsync();
                    }
                }

                return Ok(new { message = "Webhook processed." });
            }
            catch (JsonException)
            {
                return BadRequest(new { message = "Invalid JSON payload." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
