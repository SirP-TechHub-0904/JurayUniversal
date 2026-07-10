using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Website.Controllers
{
    [ApiController]
    [Route("api/paystack-webhook")]
    [Microsoft.AspNetCore.Authorization.AllowAnonymous]
    public class PaystackWebhookController : ControllerBase
    {
        private readonly DashboardDbContext _context;
        private readonly IConfiguration _configuration;

        public PaystackWebhookController(DashboardDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> HandleWebhook()
        {
            try
            {
                using var reader = new StreamReader(Request.Body);
                var rawBody = await reader.ReadToEndAsync();

                var signature = Request.Headers["x-paystack-signature"].ToString();
                bool signatureValid = false;
                bool secretsConfigured = false;

                var settings = await _context.MisSettings.FirstOrDefaultAsync();
                
                if (settings != null && !string.IsNullOrWhiteSpace(settings.PaystackWebhookSecret))
                {
                    secretsConfigured = true;
                    signatureValid = VerifySignature(rawBody, settings.PaystackWebhookSecret, signature);
                }

                var devSecret = _configuration["Paystack:SecretKey"];
                if (!signatureValid && !string.IsNullOrWhiteSpace(devSecret))
                {
                    secretsConfigured = true;
                    signatureValid = VerifySignature(rawBody, devSecret, signature);
                }

                if (secretsConfigured && !signatureValid)
                {
                    return Unauthorized(new { message = "Invalid signature." });
                }

                using var doc = JsonDocument.Parse(rawBody);
                var root = doc.RootElement;

                string eventType = null;
                if (root.TryGetProperty("event", out var eventEl))
                    eventType = eventEl.GetString();

                if (eventType != "charge.success")
                    return Ok(new { message = $"Event '{eventType}' ignored." });

                string reference = null;
                string status = null;

                if (root.TryGetProperty("data", out var data))
                {
                    if (data.TryGetProperty("reference", out var refEl)) reference = refEl.GetString();
                    if (data.TryGetProperty("status", out var statusEl)) status = statusEl.GetString()?.ToLowerInvariant();
                }

                if (string.IsNullOrEmpty(reference))
                    return BadRequest(new { message = "No reference in payload." });

                if (reference.StartsWith("SMS-"))
                {
                    return await HandleSmsWebhook(reference, status, settings);
                }
                
                if (reference.StartsWith("NEWS-"))
                {
                    return await HandleNewsletterWebhook(reference, status, settings);
                }

                var tx = await _context.MisFinanceTransactions
                    .FirstOrDefaultAsync(t => t.TransactionReference == reference);

                if (tx == null)
                    return Ok(new { message = "Reference not found ignored." });

                if (status == "success")
                {
                    if (tx.Status != PaymentStatus.Success)
                    {
                        tx.Status = PaymentStatus.Success;
                        tx.PaidAt = DateTime.UtcNow;
                        await _context.SaveChangesAsync();
                    }
                }
                else if (status == "failed" || status == "abandoned")
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

        private async Task<IActionResult> HandleSmsWebhook(string reference, string status, MisSetting settings)
        {
            var tx = await _context.MisSmsTransactions.FirstOrDefaultAsync(t => t.Reference == reference);
            if (tx == null)
                return Ok(new { message = "SMS Reference not found ignored." });

            if (status == "success")
            {
                if (tx.Status != PaymentStatus.Success)
                {
                    if (settings == null)
                    {
                        settings = new MisSetting();
                        _context.MisSettings.Add(settings);
                    }
                    settings.SmsCreditBalance += tx.Amount;
                    tx.Status = PaymentStatus.Success;
                    await _context.SaveChangesAsync();
                }
            }
            else if (status == "failed" || status == "abandoned")
            {
                if (tx.Status == PaymentStatus.Pending)
                {
                    tx.Status = PaymentStatus.Failed;
                    await _context.SaveChangesAsync();
                }
            }
            return Ok(new { message = "SMS Webhook processed." });
        }

        private async Task<IActionResult> HandleNewsletterWebhook(string reference, string status, MisSetting settings)
        {
            var tx = await _context.MisNewsletterPlanTransactions.Include(x => x.MisNewsletterPlan).FirstOrDefaultAsync(t => t.Reference == reference);
            if (tx == null)
                return Ok(new { message = "Newsletter Reference not found ignored." });

            if (status == "success")
            {
                if (tx.Status != PaymentStatus.Success)
                {
                    tx.Status = PaymentStatus.Success;
                    tx.PaidAt = DateTime.UtcNow;
                    var plan = tx.MisNewsletterPlan;
                    
                    if (tx.IsGlobalPurchase)
                    {
                        if (settings == null)
                        {
                            settings = new MisSetting();
                            _context.MisSettings.Add(settings);
                        }
                        settings.GlobalNewsletterQuota += tx.Quota;
                        var newExpiry = DateTime.UtcNow.AddMonths(plan.DurationInMonths);
                        if (settings.GlobalNewsletterQuotaExpiry == null || settings.GlobalNewsletterQuotaExpiry < DateTime.UtcNow)
                        {
                            settings.GlobalNewsletterQuotaExpiry = newExpiry;
                        }
                        else
                        {
                            settings.GlobalNewsletterQuotaExpiry = settings.GlobalNewsletterQuotaExpiry.Value.AddMonths(plan.DurationInMonths);
                        }
                    }
                    else
                    {
                        var wallet = await _context.MisNewsletterWallets.FirstOrDefaultAsync(w => w.UserId == tx.UserId);
                        if (wallet == null)
                        {
                            wallet = new MisNewsletterWallet
                            {
                                UserId = tx.UserId,
                                EmailQuotaBalance = tx.Quota,
                                ExpiryDate = DateTime.UtcNow.AddMonths(plan.DurationInMonths)
                            };
                            _context.MisNewsletterWallets.Add(wallet);
                        }
                        else
                        {
                            wallet.EmailQuotaBalance += tx.Quota;
                            var newExpiry = DateTime.UtcNow.AddMonths(plan.DurationInMonths);
                            if (wallet.ExpiryDate == null || wallet.ExpiryDate < DateTime.UtcNow)
                            {
                                wallet.ExpiryDate = newExpiry;
                            }
                            else
                            {
                                wallet.ExpiryDate = wallet.ExpiryDate.Value.AddMonths(plan.DurationInMonths);
                            }
                        }

                        var wTx = new MisNewsletterWalletTransaction
                        {
                            MisNewsletterWalletId = wallet.Id,
                            Amount = tx.Quota,
                            TransactionType = JurayUniversal.Domain.Enum.Enum.MisWalletTransactionType.Credit,
                            Description = $"Purchased Plan: {plan.Name}",
                            Date = DateTime.UtcNow
                        };
                        _context.MisNewsletterWalletTransactions.Add(wTx);
                    }
                    
                    await _context.SaveChangesAsync();
                }
            }
            else if (status == "failed" || status == "abandoned")
            {
                if (tx.Status == PaymentStatus.Pending)
                {
                    tx.Status = PaymentStatus.Failed;
                    await _context.SaveChangesAsync();
                }
            }
            return Ok(new { message = "Newsletter Webhook processed." });
        }

        private static bool VerifySignature(string payload, string secret, string signature)
        {
            using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(secret));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
            var computed = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            return computed == signature?.ToLowerInvariant();
        }
    }
}
