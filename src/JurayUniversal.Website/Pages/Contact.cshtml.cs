using JurayUniversal.Application.Dtos;
using JurayUniversal.Application.Repository.NotifyRegister;
using JurayUniversal.Application.Repository.Services;
using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Web.Helpers;

namespace JurayUniversal.Website.Pages
{
    public class ContactModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly ISettingsService _settingsService;
        private readonly IEmailSendService _email;

        public ContactModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, ISettingsService settingsService, IEmailSendService email)
        {
            _context = context;
            _settingsService = settingsService;
            _email = email;
        }
        public SuperSetting SuperSetting { get; set; }
        public Setting Setting { get; set; }

        [BindProperty]
        public ClientRequest ClientRequest { get; set; }

        [BindProperty]
        public int FirstNumber { get; set; }

        [BindProperty]
        public int SecondNumber { get; set; }


        [BindProperty]
        public int Captcha { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Setting = await _context.Settings.FirstOrDefaultAsync();
            var httpContext = HttpContext;
            VerificationWebDto setting = await _settingsService.ValidateWeb(httpContext);
            if (setting.SettingFound == false)
            {
                return RedirectToPage(setting.Path, new { area = setting.Area });
            }
            if (setting.Portfolio == true)
            {
                return RedirectToPage(setting.PortfolioPath);

            }

            SuperSetting = setting.SuperSetting;

            Random random = new Random();

            // Generate a random integer between 0 and 9
            FirstNumber = random.Next(0, 10);
            SecondNumber = random.Next(0, 10);
            return Page();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            Setting = await _context.Settings.FirstOrDefaultAsync();
            var httpContext = HttpContext;
            VerificationWebDto setting = await _settingsService.ValidateWeb(httpContext);
            SuperSetting = setting.SuperSetting;
            if (FirstNumber + SecondNumber != Captcha)
            {



                TempData["error"] = "Invalid Submission";
                return Page();
            }

            string mailtext = @"
 
    <table role=""presentation"" cellspacing=""0"" cellpadding=""0"" width=""100%"" style=""border-collapse: collapse;"">
        <tr>
            <td style=""padding: 20px 0; text-align: center;"">
                <h4>SOMEONE CONTACTED YOU FROM YOUR WEBSITE.</h4>
            </td>
        </tr>
        <tr>
            <td style=""padding: 20px;"">
SUBJECT: @@subject@@<br><br>
NAME: @@name@@<br><br>
EMAIL: @@email@@<br><br>
MESSAGE: @@message@@

            </td>
        </tr>
     

    </table>
 
";
            mailtext = mailtext.Replace("@@subject@@", ClientRequest.Subject);
            mailtext = mailtext.Replace("@@name@@", ClientRequest.Name);
            mailtext = mailtext.Replace("@@email@@", ClientRequest.Email);
            mailtext = mailtext.Replace("@@message@@", ClientRequest.Message);

            //"NIPSS <admin@nipssportal.com.ng>"
            await _email.SendEmailPostmasterAddFrom($"{SuperSetting.CompanyName} <client@juray.ng>", SuperSetting.CompanyName, Setting.EmailOne, "", "", $"WEBSITE CONTACT US FORM", mailtext);

            var dashboardsetting = await _context.DashboardSettings.FirstOrDefaultAsync();
            if (dashboardsetting.ActivateDashboard == true)
            {

                _context.ClientRequest.Add(ClientRequest);
                await _context.SaveChangesAsync();
            }
            TempData["success"] = "Successful";

            return RedirectToPage("/ThankYou");
        }
    }
}
