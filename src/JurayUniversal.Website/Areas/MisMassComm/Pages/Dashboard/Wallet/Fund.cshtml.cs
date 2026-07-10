using JurayUniversal.Application.Services;
using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using static JurayUniversal.Domain.Enum.Enum;

namespace JurayUniversal.Website.Areas.MisMassComm.Pages.Dashboard.Wallet
{
    [Authorize(Roles = "mSuperAdmin,mAdmin,MisAdmin")]
    public class FundModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;
        private readonly PixelPayService _pixelPayService;
        private readonly PaystackService _paystackService;

        public FundModel(
            DashboardDbContext context,
            UserManager<Profile> userManager,
            PixelPayService pixelPayService,
            PaystackService paystackService)
        {
            _context = context;
            _userManager = userManager;
            _pixelPayService = pixelPayService;
            _paystackService = paystackService;
        }

        [BindProperty]
        public decimal AmountToFund { get; set; }

        [BindProperty]
        public string PaymentProvider { get; set; }

        public decimal CurrentBalance { get; set; }

        public bool PixelPayAvailable { get; set; }
        public bool PaystackAvailable { get; set; }
        
        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var settings = await _context.MisSettings.FirstOrDefaultAsync();
            CurrentBalance = settings?.SmsCreditBalance ?? 0m;
            ResolveAvailableProviders(settings);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (AmountToFund <= 0)
            {
                StatusMessage = "Error: Amount must be greater than zero.";
                return await OnGetAsync();
            }

            var settings = await _context.MisSettings.FirstOrDefaultAsync();
            ResolveAvailableProviders(settings);

            var provider = (PaymentProvider ?? "").Trim();
            if ((provider == "PixelPay" && !PixelPayAvailable) ||
                (provider == "Paystack" && !PaystackAvailable) ||
                (provider != "PixelPay" && provider != "Paystack"))
            {
                StatusMessage = "Error: Invalid or unavailable payment provider selected.";
                return await OnGetAsync();
            }

            var user = await _userManager.GetUserAsync(User);
            var userId = user?.Id;
            var email = user?.Email ?? "guest@payment.com";

            var transactionRef = $"SMS-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString("N")[..6].ToUpper()}";

            var transaction = new MisSmsTransaction
            {
                Amount = AmountToFund,
                TransactionType = JurayUniversal.Domain.Enum.Enum.MisWalletTransactionType.Credit,
                Description = "Online SMS Wallet Top-up",
                Reference = transactionRef,
                CreatedAt = DateTime.UtcNow,
                Status = PaymentStatus.Pending,
                PaymentProvider = provider,
                UserId = userId
            };

            _context.MisSmsTransactions.Add(transaction);
            await _context.SaveChangesAsync();

            var callbackUrl = $"{Request.Scheme}://{Request.Host}/pay/callback?ref={transactionRef}";
            PaymentInitResult result;

            if (provider == "Paystack")
            {
                result = await _paystackService.InitializePaymentAsync(transactionRef, AmountToFund, email, callbackUrl);
            }
            else
            {
                result = await _pixelPayService.InitializePaymentAsync(transactionRef, AmountToFund, null, callbackUrl);
            }

            if (!result.Success)
            {
                StatusMessage = $"Error: {result.ErrorMessage}";
                return await OnGetAsync();
            }

            return Redirect(result.PaymentUrl);
        }

        private void ResolveAvailableProviders(JurayUniversal.Domain.Models.MisSetting s)
        {
            if (s == null) return;
            PixelPayAvailable = s.EnablePixelPay
                && !string.IsNullOrWhiteSpace(s.PixelPayKeyId)
                && !string.IsNullOrWhiteSpace(s.PixelPaySecretKey)
                && !string.IsNullOrWhiteSpace(s.PixelPayBaseUrl);

            PaystackAvailable = s.EnablePaystack
                && !string.IsNullOrWhiteSpace(s.PaystackSecretKey);
        }
    }
}
