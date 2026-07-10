using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisConfiguration.Pages.Dashboard.MisSettings
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public IndexModel(DashboardDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public JurayUniversal.Domain.Models.MisSetting Setting { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Setting = await _context.MisSettings.FirstOrDefaultAsync();
            if (Setting == null)
            {
                Setting = new JurayUniversal.Domain.Models.MisSetting { CategoryAlias = "Category", SubCategoryAlias = "Sub-Category" };
                _context.MisSettings.Add(Setting);
                await _context.SaveChangesAsync();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var existing = await _context.MisSettings.FirstOrDefaultAsync();
            if (existing != null)
            {
                // Entity is already tracked by EF — just update the properties directly
                existing.CategoryAlias = Setting.CategoryAlias;
                existing.SubCategoryAlias = Setting.SubCategoryAlias;
                existing.GroupAlias = Setting.GroupAlias;
                existing.AlumniAlias = Setting.AlumniAlias;
                existing.EnableListDownload = Setting.EnableListDownload;
                
                existing.PixelPayBaseUrl = Setting.PixelPayBaseUrl;
                existing.PixelPayKeyId = Setting.PixelPayKeyId;
                existing.PixelPaySecretKey = Setting.PixelPaySecretKey;
                existing.PixelPaySignature = Setting.PixelPaySignature;
                existing.PixelPayCurrency = Setting.PixelPayCurrency;
                existing.PixelPayWebhookSecret = Setting.PixelPayWebhookSecret;
                existing.EnablePixelPay = Setting.EnablePixelPay;

                existing.EnablePaystack = Setting.EnablePaystack;
                existing.PaystackPublicKey = Setting.PaystackPublicKey;
                existing.PaystackSecretKey = Setting.PaystackSecretKey;
                existing.PaystackWebhookSecret = Setting.PaystackWebhookSecret;

                // ZeptoMail Settings
                existing.EnableZeptoMail = Setting.EnableZeptoMail;
                existing.ZeptoMailApiKey = Setting.ZeptoMailApiKey;
                existing.ZeptoMailSenderEmail = Setting.ZeptoMailSenderEmail;
                existing.ZeptoMailSenderName = Setting.ZeptoMailSenderName;
                existing.ZeptoMailSendMailId = Setting.ZeptoMailSendMailId;
                existing.EmailMaxRetries = Setting.EmailMaxRetries;

                // Captcha Settings
                existing.ActiveCaptcha = Setting.ActiveCaptcha;
                existing.GoogleRecaptchaSiteKey = Setting.GoogleRecaptchaSiteKey;
                existing.GoogleRecaptchaSecretKey = Setting.GoogleRecaptchaSecretKey;

                _context.MisSettings.Update(existing);
            }
            else
            {
                _context.MisSettings.Add(Setting);
            }

            await _context.SaveChangesAsync();

            StatusMessage = "MIS Settings successfully updated.";
            return RedirectToPage();
        }
    }
}

