using JurayUniversal.Application.Services;
using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Website.Pages.Pay
{
    /// <summary>
    /// Callback page: payment gateway redirects the user here after payment.
    /// PixelPay  : /pay/callback?ref=TX-...&status=PAID
    /// Paystack  : /pay/callback?ref=TX-...&reference=TX-... (uses ?ref we set as callback_url param)
    /// </summary>
    [Microsoft.AspNetCore.Authorization.AllowAnonymous]
    public class CallbackModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly PixelPayService _pixelPayService;
        private readonly PaystackService _paystackService;

        public CallbackModel(
            DashboardDbContext context,
            PixelPayService pixelPayService,
            PaystackService paystackService)
        {
            _context = context;
            _pixelPayService = pixelPayService;
            _paystackService = paystackService;
        }

        public MisFinanceTransaction Transaction { get; set; }
        public MisSmsTransaction SmsTransaction { get; set; }
        public SuperSetting SuperSetting { get; set; }
        public bool IsSuccess { get; set; }

        public async Task<IActionResult> OnGetAsync(string @ref, string status = null, string reference = null)
        {
            SuperSetting = await _context.SuperSettings.FirstOrDefaultAsync() ?? new SuperSetting();

            // Support both ?ref= (our param) and ?reference= (Paystack appends this)
            var txRef = @ref ?? reference;
            if (string.IsNullOrEmpty(txRef)) return RedirectToPage("/pay/Index");

            if (txRef.StartsWith("SMS-"))
            {
                return await HandleSmsTransactionAsync(txRef, status);
            }

            Transaction = await _context.MisFinanceTransactions
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.TransactionReference == txRef);

            if (Transaction == null) return NotFound();

            if (Transaction.Status == PaymentStatus.Success)
            {
                IsSuccess = true;
                return Page();
            }

            var provider = (Transaction.PaymentProvider ?? "PixelPay").Trim();

            if (provider == "Paystack")
            {
                var verified = await _paystackService.VerifyPaymentAsync(txRef);
                if (verified)
                {
                    Transaction.Status = PaymentStatus.Success;
                    Transaction.PaidAt = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                    IsSuccess = true;
                }
                else
                {
                    var paystackStatus = (status ?? "").ToUpperInvariant();
                    if (paystackStatus == "FAILED" || paystackStatus == "ABANDONED")
                    {
                        Transaction.Status = PaymentStatus.Failed;
                        await _context.SaveChangesAsync();
                    }
                    IsSuccess = Transaction.Status == PaymentStatus.Success;
                }
            }
            else
            {
                var pixelStatus = (status ?? "").ToUpperInvariant();
                if (pixelStatus == "PAID" || pixelStatus == "SUCCESS" || pixelStatus == "00")
                {
                    Transaction.Status = PaymentStatus.Success;
                    Transaction.PaidAt = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                    IsSuccess = true;
                }
                else if (pixelStatus == "FAILED" || pixelStatus == "DECLINED" || pixelStatus == "05")
                {
                    Transaction.Status = PaymentStatus.Failed;
                    await _context.SaveChangesAsync();
                    IsSuccess = false;
                }
                else
                {
                    IsSuccess = Transaction.Status == PaymentStatus.Success;
                }
            }

            return Page();
        }

        private async Task<IActionResult> HandleSmsTransactionAsync(string txRef, string status)
        {
            SmsTransaction = await _context.MisSmsTransactions.FirstOrDefaultAsync(t => t.Reference == txRef);
            if (SmsTransaction == null) return NotFound();

            if (SmsTransaction.Status == PaymentStatus.Success)
            {
                IsSuccess = true;
                return Page();
            }

            var provider = (SmsTransaction.PaymentProvider ?? "PixelPay").Trim();

            if (provider == "Paystack")
            {
                var verified = await _paystackService.VerifyPaymentAsync(txRef);
                if (verified)
                {
                    await CreditSmsWallet(SmsTransaction.Amount);
                    SmsTransaction.Status = PaymentStatus.Success;
                    await _context.SaveChangesAsync();
                    IsSuccess = true;
                }
                else
                {
                    var paystackStatus = (status ?? "").ToUpperInvariant();
                    if (paystackStatus == "FAILED" || paystackStatus == "ABANDONED")
                    {
                        SmsTransaction.Status = PaymentStatus.Failed;
                        await _context.SaveChangesAsync();
                    }
                    IsSuccess = SmsTransaction.Status == PaymentStatus.Success;
                }
            }
            else
            {
                var pixelStatus = (status ?? "").ToUpperInvariant();
                if (pixelStatus == "PAID" || pixelStatus == "SUCCESS" || pixelStatus == "00")
                {
                    await CreditSmsWallet(SmsTransaction.Amount);
                    SmsTransaction.Status = PaymentStatus.Success;
                    await _context.SaveChangesAsync();
                    IsSuccess = true;
                }
                else if (pixelStatus == "FAILED" || pixelStatus == "DECLINED" || pixelStatus == "05")
                {
                    SmsTransaction.Status = PaymentStatus.Failed;
                    await _context.SaveChangesAsync();
                    IsSuccess = false;
                }
                else
                {
                    IsSuccess = SmsTransaction.Status == PaymentStatus.Success;
                }
            }
            return Page();
        }

        private async Task CreditSmsWallet(decimal amount)
        {
            var settings = await _context.MisSettings.FirstOrDefaultAsync();
            if (settings == null)
            {
                settings = new MisSetting();
                _context.MisSettings.Add(settings);
            }
            settings.SmsCreditBalance += amount;
        }
    }
}
