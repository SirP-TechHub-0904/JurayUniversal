using JurayUniversal.Application.Services;
using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisMassComm.Pages.Platform.Wallet
{
    public class FundModel : PageModel
    {
        private readonly DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;
        private readonly PaystackService _paystackService;

        public FundModel(DashboardDbContext context, UserManager<Profile> userManager, PaystackService paystackService)
        {
            _context = context;
            _userManager = userManager;
            _paystackService = paystackService;
        }

        [BindProperty]
        public decimal Amount { get; set; }

        public MisWallet UserWallet { get; set; } = new MisWallet();
        public string? StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var wallet = await _context.MisWallets.FirstOrDefaultAsync(w => w.UserId == user.Id);
            if (wallet == null)
            {
                wallet = new MisWallet { UserId = user.Id, Balance = 0 };
                _context.MisWallets.Add(wallet);
                await _context.SaveChangesAsync();
            }
            UserWallet = wallet;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            if (Amount < 100)
            {
                StatusMessage = "Minimum funding amount is 100 NGN.";
                return await OnGetAsync();
            }

            var settings = await _context.MisSettings.FirstOrDefaultAsync();
            if (settings == null || !settings.EnablePaystack || string.IsNullOrEmpty(settings.PaystackSecretKey))
            {
                StatusMessage = "Payment gateway is currently not configured.";
                return await OnGetAsync();
            }

            string transactionRef = "MC_FUND_" + Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper();

            // Save pending transaction if needed, but we'll create it directly on Verify to avoid clutter.
            var callbackUrl = $"{Request.Scheme}://{Request.Host}/MisMassComm/Platform/Wallet/VerifyFund?reference={transactionRef}&amt={Amount}";
            
            var email = user.Email ?? "user@system.com";
            
            var result = await _paystackService.InitializePaymentAsync(transactionRef, Amount, email, callbackUrl);

            if (!result.Success)
            {
                StatusMessage = $"Error initializing payment: {result.ErrorMessage}";
                return await OnGetAsync();
            }

            return Redirect(result.PaymentUrl);
        }
    }
}
