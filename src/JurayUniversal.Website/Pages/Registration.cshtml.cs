using DocumentFormat.OpenXml.Spreadsheet;
using JurayUniversal.Application.Dtos;
using JurayUniversal.Application.Repository.NotifyRegister;
using JurayUniversal.Application.Repository.Services;
using JurayUniversal.Domain.Models;
using JurayUniversal.Domain.Models.NIPSS;
using JurayUniversal.Website.Areas.NIPSSADMIN.Pages.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;
namespace JurayUniversal.Website.Pages
{
    public class RegistrationModel : PageModel
     {
        private readonly UserManager<Profile> _userManager;
        private readonly SignInManager<Profile> _signInManager;
        private readonly ILogger<RegistrationModel> _logger;
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly RoleManager<IdentityRole> _role;
        private readonly IEmailSendService _email;
        private readonly ISmsSendService _sms;
        private readonly ISettingsService _settingsService;

        public RegistrationModel(SignInManager<Profile> signInManager,
            ILogger<RegistrationModel> logger,
            UserManager<Profile> userManager,
            JurayUniversal.Persistence.EF.SQL.DashboardDbContext context,
            RoleManager<IdentityRole> role,
            IEmailSendService email,
            ISmsSendService sms,
            ISettingsService settingsService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
            _role = role;
            _email = email;
            _sms = sms;
            _settingsService = settingsService;
        }
        public string Templatechoose { get; set; }
        public Setting Setting { get; set; }
        public SuperSetting SuperSetting { get; set; }
        [BindProperty]
        public Profile UserDatas { get; set; }

        [BindProperty]
        public string Role { get; set; }
        public Course Course { get; set; }

        [BindProperty]
        public long? CourseId { get;set;}
        public async Task<IActionResult> OnGetAsync(long? mx, long? sec)
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

            if (sec == null)
            {
                return NotFound();
            }
            ViewData["NameTitle"] = new SelectList(_context.TitleLists, "Title", "Title");
            var lx = await _context.Courses.ToListAsync();
            Course = await _context.Courses.Include(x => x.CourseCategory).FirstOrDefaultAsync(x=>x.Id == mx);
            if(Course == null)
            {
                return NotFound();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {

            //if (ModelState.IsValid)
            //{
            string password = GenerateRandomAlphaNumeric(8);


            var user = new Profile
            {
                UserName = UserDatas.Email,
                Email = UserDatas.Email,
                PhoneNumber = UserDatas.PhoneNumber,
                FirstName = UserDatas.FirstName,
                LastName = UserDatas.LastName,
                MiddleName = UserDatas.MiddleName,
                Date = DateTime.UtcNow.AddHours(1),
                Title = UserDatas.Title,
                Role = "Participant",
                ResetPassword = true,
                TempPass = password,
                UpdateProfile = true,
                UpdateAwards = true,
                UpdateCertificate = true,
                UpdateEducation = true,
                UpdateExperience = true,
                UpdateInterest = true,
                UpdateLanguage = true,
                UpdateReference = true,
                UpdateSkills = true,
                LockoutEnabled = false,
                UserStatus = Domain.Enum.Enum.UserStatus.Active,
                PositionOrRank = UserDatas.PositionOrRank,
            };
            while (UserDatas.UniqueId == null)
            {
                string uniqId = GenerateRandomAlphaNumericUniqueId(8);

                // Check if the generated IdDigit already exists in the database
                var check = await _userManager.Users.FirstOrDefaultAsync(x => x.UniqueId == uniqId);

                if (check == null)
                {
                    // If not found, update the UserDatas and exit the loop
                    user.UniqueId = uniqId;
                    break;
                }

            }

            user.Id = Guid.NewGuid().ToString();

            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");
                //
                Role = "Participant";
                var roleExists = await _role.RoleExistsAsync(Role);
                if (!roleExists)
                {
                    return BadRequest($"The role '{Role}' does not exist.");
                }
                await _userManager.AddToRoleAsync(user, Role);
                try
                {
                    List<string> category = new List<string> { "EDUCATION", "EXPERIENCE", "CERTIFICATIONS", "SKILLS", "LANGUAGES", "AWARDS", "INTEREST", "REFERENCES" };

                    foreach (string item in category)
                    {
                        ProfileCategory p = new ProfileCategory();
                        p.ProfileId = user.Id;
                        p.Title = item;
                        _context.ProfileCategories.Add(p);
                        await _context.SaveChangesAsync();

                    }
                }
                catch (Exception c)
                {

                }
                //Message newmessage = new Message();
                //newmessage.
                try { 
                var linkcomplete = Url.Page(
                    "/AuthV2/Account/Login",
                    pageHandler: null,
                    values: new { area = "V2" },
                    protocol: Request.Scheme);
                string datavalue = $"<a href='{HtmlEncoder.Default.Encode(linkcomplete)}'>click here to login</a>.";

                var getusertoupdate = await _userManager.FindByIdAsync(user.Id);
                string messagedetails = $"Login Email: {user.Email} <br>Temporal Password: {password}<br>" +
                    $"" + datavalue + "or copy the link before and paste in your browser to continue<br><br>" +
                    linkcomplete + "<br>" +
                    "<h4>INSTRUCTION</h4>" +

                    $"<h6>Kindly login, reset your password and update your profile to enable you choose your accommodation.</h6>";



                var send = await _sms.SendSmsWithResult(user.PhoneNumber, "Kindly check your email to complete your registration.");
               
                if (Role == "Participant")
                {
                    var sendmailparticipants = await _email.SendEmailWithResult(user.FullnameX, user.Email, "", "", $"NIPSS PARTICIPANT REGISTRATION", messagedetails);

                    if (send == true)
                    {
                        getusertoupdate.SmsSent = true;
                    }
                    if (sendmailparticipants == true)
                    {
                        getusertoupdate.EmailSent = true;
                    }
                    await _userManager.UpdateAsync(getusertoupdate);
                    
                ///
                ///
                ///CourseId
                ///
                // Get the last participant for the specific CourseId
                var lastParticipant = await _context.ParticipantLists
                        .Where(p => p.CourseId == CourseId)
                        .OrderByDescending(p => p.Id)
                        .FirstOrDefaultAsync();

                    int newnumber = 0;

                    if (lastParticipant == null)
                    {
                        newnumber = 1;

                    }
                    else
                    {
                        newnumber = lastParticipant.SerialNumber + 1;

                    }
                    var getcourse = await _context.Courses.Include(x => x.CourseCategory).FirstOrDefaultAsync(x => x.Id == CourseId);



                    ParticipantList newParticipant = new ParticipantList();
                    newParticipant.IdNumber = $"NIPSS/{getcourse.CourseCategory.Abbreviation}/{getcourse.SecNumber}/{getcourse.Year.ToString("yy")}/{newnumber.ToString("000")}";
                    newParticipant.SerialNumber = newnumber;
                    newParticipant.CourseId = CourseId;
                    newParticipant.UserId = user.Id;
                    newParticipant.ParticipantStatus = Domain.Enum.Enum.ParticipantStatus.Active;
                    _context.ParticipantLists.Add(newParticipant);
                    await _context.SaveChangesAsync();



                    var getparticipant = await _context.ParticipantLists.FirstOrDefaultAsync(x => x.Id == newParticipant.Id);
                     
                    //send sms
                    TempData["success"] = "Successful";
                    return RedirectToPage("./RegStatus", new { myx = newParticipant.Id, id = Guid.NewGuid() }); 
                }
                 
                var sendmail = await _email.SendEmailWithResult(user.FirstName, user.Email, "", "", $"NIPSS REGISTRATION", messagedetails);
                if (send == true)
                {
                    getusertoupdate.SmsSent = true;
                }
                if (sendmail == true)
                {
                    getusertoupdate.EmailSent = true;
                }
                await _userManager.UpdateAsync(getusertoupdate);
                if (user.Role == "Participant")
                {
                    return RedirectToPage("./RegistrationStatus", new { id = user.Id });
                }
                }
                catch (Exception c)
                {

                }
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            //}
            ViewData["NameTitle"] = new SelectList(_context.TitleLists, "Title", "Title");
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
 
              Course = await _context.Courses.Include(x => x.CourseCategory).FirstOrDefaultAsync(x => x.Id == CourseId);
            if (Course == null)
            {
                return NotFound();
            }
            return Page();
        }
        static string GenerateRandomAlphaNumeric(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        static string GenerateRandomAlphaNumericUniqueId(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }

}
