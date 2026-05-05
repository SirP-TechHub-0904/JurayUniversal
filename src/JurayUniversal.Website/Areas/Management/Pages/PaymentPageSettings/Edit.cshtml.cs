using System.Threading.Tasks;
using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.Management.Pages.PaymentPageSettings
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin")]
    public class EditModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public EditModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public SuperSetting SuperSetting { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            SuperSetting = await _context.SuperSettings.FirstOrDefaultAsync();

            if (SuperSetting == null)
            {
                SuperSetting = new SuperSetting();
                _context.SuperSettings.Add(SuperSetting);
                await _context.SaveChangesAsync();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            

            var setting = await _context.SuperSettings.FirstOrDefaultAsync();

            if (setting == null)
            {
                _context.SuperSettings.Add(SuperSetting);
            }
            else
            {
                setting.PaystackPublicKey = SuperSetting.PaystackPublicKey;
                setting.PaystackSecretKey = SuperSetting.PaystackSecretKey;
                setting.PaystackCallbackUrl = SuperSetting.PaystackCallbackUrl;
                setting.PaymentModuleActive = SuperSetting.PaymentModuleActive;
                setting.CurrencySymbol = SuperSetting.CurrencySymbol;
                setting.CurrencyCode = SuperSetting.CurrencyCode;
            }

            await _context.SaveChangesAsync();
            TempData["success"] = "Payment settings updated successfully";
            return RedirectToPage("./Index");
        }
    }
}