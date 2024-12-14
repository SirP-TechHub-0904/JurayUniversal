using JurayUniversal.Application.Repository.NotifyRegister;
using JurayUniversal.Domain.Models;
using JurayUniversal.Domain.Models.NIPSS;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.Encodings.Web;
using System.Web.Helpers;

namespace JurayUniversal.Website.Areas.NIPSSADMIN.Pages.Users
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Admin")]

    public class StaffStatusModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly IEmailSendService _email;
        private readonly ISmsSendService _sms;
        private readonly UserManager<Profile> _userManager;

        public StaffStatusModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, IEmailSendService email, ISmsSendService sms, UserManager<Profile> userManager)
        {
            _context = context;
            _email = email;
            _sms = sms;
            _userManager = userManager;
        }

        [BindProperty]
        public Profile UserData { get; set; }

        [BindProperty]
        public string UserId { get; set; }
        [BindProperty]
        public string ParticipantId { get; set; }

        [BindProperty]
        public bool SendEmail { get; set; }

        public StaffManager StaffManage { get; set; }
        [BindProperty]
        public bool SendSMS { get; set; }
        public async Task<IActionResult> OnGetAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            UserData = user;
            StaffManage = await _context.StaffManagers.FirstOrDefaultAsync(x=>x.UserId == user.Id);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var user = await _userManager.FindByIdAsync(UserId);


                var linkcomplete = Url.Page(
                    "/AuthV2/Account/Login",
                    pageHandler: null,
                    values: new { area = "V2" },
                    protocol: Request.Scheme);
                string datavalue = $"<a href='{HtmlEncoder.Default.Encode(linkcomplete)}'>click here to login</a>.";

                var getusertoupdate = await _userManager.FindByIdAsync(user.Id);
                string messagedetails = $"Login Email: {user.Email} <br>Temporal Password: {user.TempPass}<br>" +
                    $"" + datavalue + "or copy the link before and paste in your browser to continue<br><br>" +
                    linkcomplete + "<br>" +
                    "<h4>INSTRUCTION</h4>" +

                    $"<h6>Kindly login, reset your password and update your profile to enable you choose your accommodation.</h6>";

                if (SendSMS == true)
                {
                    var send = await _sms.SendSmsWithResult(user.PhoneNumber, "Kindly check your email to complete your registration.");
                    if (send == true)
                    {
                        user.SmsSent = true;
                        await _userManager.UpdateAsync(user);
                    }
                }
                if (SendEmail == true)
                { 
                        //var sendmail = await _email.SendEmailWithResult(user.FirstName, user.Email, "", "", $"NIPSS REGISTRATION", messagedetails);
                        var sendmail = await _email.SendEmailPostmasterReturn(user.FirstName, user.Email, "", "", $"NIPSS REGISTRATION", messagedetails);
                    
                        if (sendmail == true)
                        {
                            user.EmailSent = true;
                        }
                        await _userManager.UpdateAsync(user);
                    
                }
                return RedirectToPage("./StaffStatus", new { id = user.Id });

            }
            catch (Exception ex)
            {

                return Page();
            }
        }

    }
}
