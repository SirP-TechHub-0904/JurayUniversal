using JurayUniversal.Application.Repository.NotifyRegister;
using JurayUniversal.Domain.Models;
using JurayUniversal.Domain.Models.NIPSS;
using JurayUniversal.Persistence.EF.SQL.Migrations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text.Encodings.Web;
using System.Web.Helpers;

namespace JurayUniversal.Website.Areas.NIPSSADMIN.Pages.Users
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Admin")]

    public class AddModel : PageModel
    {
        private readonly UserManager<Profile> _userManager;
        private readonly SignInManager<Profile> _signInManager;
        private readonly ILogger<IndexModel> _logger;
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly RoleManager<IdentityRole> _role;
        private readonly IEmailSendService _email;
        private readonly ISmsSendService _sms;

        public AddModel(SignInManager<Profile> signInManager,
            ILogger<IndexModel> logger,
            UserManager<Profile> userManager,
            JurayUniversal.Persistence.EF.SQL.DashboardDbContext context,
            RoleManager<IdentityRole> role,
            IEmailSendService email,
            ISmsSendService sms)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
            _role = role;
            _email = email;
            _sms = sms;
        }

        [BindProperty]
        public Profile UserDatas { get; set; }

        [BindProperty]
        public string Role { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            ViewData["NameTitle"] = new SelectList(_context.TitleLists, "Title", "Title");

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
                Role = Role,
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
                IdentityRole Managerf = new IdentityRole(Role);
                var checkManagerf = await _role.FindByNameAsync(Role);
                if (checkManagerf == null)
                {
                    await _role.CreateAsync(Managerf);

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

                    return RedirectToPage("./Course", new { id = user.Id });
                }
                else 
                {
                     StaffManager staff = new StaffManager();
                    staff.UserId
                        = user.Id;
                    _context.StaffManagers.Add(staff);
                    await _context.SaveChangesAsync();
                    var getstaff = await _context.StaffManagers.FirstOrDefaultAsync(x=>x.Id == staff.Id);
                    getstaff.StaffId = $"NIPSS/{staff.Id.ToString("0000")}";

                    _context.Attach(getstaff).State = EntityState.Modified;

                    await _context.SaveChangesAsync();
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
                else  
                {
                    return RedirectToPage("./StaffStatus", new { id = user.Id });

                }
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            //}
            ViewData["NameTitle"] = new SelectList(_context.TitleLists, "Title", "Title");

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
