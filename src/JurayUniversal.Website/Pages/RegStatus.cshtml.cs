using JurayUniversal.Application.Repository.NotifyRegister;
using JurayUniversal.Domain.Models.NIPSS;
using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;
using JurayUniversal.Application.Dtos;
using JurayUniversal.Application.Repository.Services;

namespace JurayUniversal.Website.Pages
{
         public class RegStatusModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly IEmailSendService _email;
        private readonly ISmsSendService _sms;
        private readonly UserManager<Profile> _userManager;
        private readonly ISettingsService _settingsService;

        public RegStatusModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, IEmailSendService email, ISmsSendService sms, UserManager<Profile> userManager, ISettingsService settingsService)
        {
            _context = context;
            _email = email;
            _sms = sms;
            _userManager = userManager;
            _settingsService = settingsService;
        }

        public ParticipantList ParticipantList { get; set; }
        [BindProperty]
        public string UserId { get; set; }
        [BindProperty]
        public string ParticipantId { get; set; }

        [BindProperty]
        public bool SendEmail { get; set; }

        public string Templatechoose { get; set; }
        public Setting Setting { get; set; }
        public SuperSetting SuperSetting { get; set; }
        [BindProperty]
        public bool SendSMS { get; set; }
        public Course Course { get; set; }

        public async Task<IActionResult> OnGetAsync(long? myx, string id)
        {
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

            Templatechoose = setting.SuperSetting.TemplateLayoutKey;
            SuperSetting = setting.SuperSetting;
            Setting = await _context.Settings.FirstOrDefaultAsync();

            if (myx == null)
            {
                return NotFound();
            }

            ParticipantList = await _context.ParticipantLists.Include(x => x.Course).ThenInclude(x => x.CourseCategory).Include(x => x.User).FirstOrDefaultAsync(m => m.Id == myx);

            if (ParticipantList == null)
            {
                return NotFound();
            }
            Course = await _context.Courses.Include(x => x.CourseCategory).FirstOrDefaultAsync(x => x.Id == ParticipantList.CourseId);
            if (Course == null)
            {
                return NotFound();
            }
            return Page();
        }

 
    }

}
