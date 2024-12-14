using JurayUniversal.Application.Services.AWS;
using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.Content.Pages.WebSetting
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,SubAdmin")]

    public class DesignSettingsModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly IConfiguration _config;
        private readonly IStorageService _storageService;

        public DesignSettingsModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, IConfiguration config, IStorageService storageService)
        {
            _context = context;
            _config = config;
            _storageService = storageService;
        }


        [BindProperty]
        public Setting Setting { get; set; }
        [BindProperty]
        public DataConfig DataConfig { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {

            Setting = await _context.Settings.FirstOrDefaultAsync();

            if (Setting == null)
            {
                return NotFound();
            }
            DataConfig = await _context.DataConfigs.FirstOrDefaultAsync();

            if (DataConfig == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var updatesetting = await _context.Settings.FirstOrDefaultAsync();
            var superupdate = await _context.DataConfigs.FirstOrDefaultAsync();

            ///

            superupdate.DashboardCSS = DataConfig.DashboardCSS;
            superupdate.LayoutCSS = DataConfig.LayoutCSS;
            superupdate.LoginCSS = DataConfig.LoginCSS;
            superupdate.Configuration = DataConfig.Configuration;
            superupdate.RedirectTohttpswww = DataConfig.RedirectTohttpswww;
            superupdate.RedirectTohttps = DataConfig.RedirectTohttps;
            superupdate.LiveConfiguration = DataConfig.LiveConfiguration;

            superupdate.LayoutJavaScript = DataConfig.LayoutJavaScript;
            if (User.IsInRole("mSuperAdmin"))
            {
                superupdate.MailSender = DataConfig.MailSender;
                superupdate.OutlookServer = DataConfig.OutlookServer;
                superupdate.OutlookPort = DataConfig.OutlookPort;
                superupdate.OutlookUsername = DataConfig.OutlookUsername;
                superupdate.OutlookSecurity = DataConfig.OutlookSecurity;
                superupdate.OutlookSenderEmail = DataConfig.OutlookSenderEmail;
                superupdate.OutlookSSLEnable = DataConfig.OutlookSSLEnable;
                superupdate.OutlookUseDefaultCredentials = DataConfig.OutlookUseDefaultCredentials;


                superupdate.GoogleServer = DataConfig.GoogleServer;
                superupdate.GooglePort = DataConfig.GooglePort;
                superupdate.GoogleUsername = DataConfig.GoogleUsername;
                superupdate.GoogleSecurity = DataConfig.GoogleSecurity;
                superupdate.GoogleSenderEmail = DataConfig.GoogleSenderEmail;
                superupdate.GoogleSSLEnable = DataConfig.GoogleSSLEnable;
                superupdate.GoogleUseDefaultCredentials = DataConfig.GoogleUseDefaultCredentials;


                superupdate.WebmailServer = DataConfig.WebmailServer;
                superupdate.WebmailPort = DataConfig.WebmailPort;
                superupdate.WebmailUsername = DataConfig.WebmailUsername;
                superupdate.WebmailSecurity = DataConfig.WebmailSecurity;
                superupdate.WebmailSenderEmail = DataConfig.WebmailSenderEmail;
                superupdate.WebmailSSLEnable = DataConfig.WebmailSSLEnable;
                superupdate.WebmailUseDefaultCredentials = DataConfig.WebmailUseDefaultCredentials;


                superupdate.CCmails = DataConfig.CCmails;
                superupdate.BBmails = DataConfig.BBmails;

            }
            try
            {
                _context.Attach(superupdate).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception g) { }

            TempData["success"] = "Successful";


            return RedirectToPage("./Index");
        }


    }
}
