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

    public class RegistrationStatusModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly IEmailSendService _email;
        private readonly ISmsSendService _sms;
        private readonly UserManager<Profile> _userManager;

        public RegistrationStatusModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, IEmailSendService email, ISmsSendService sms, UserManager<Profile> userManager)
        {
            _context = context;
            _email = email;
            _sms = sms;
            _userManager = userManager;
        }

        public ParticipantList ParticipantList { get; set; }
        [BindProperty]
        public string UserId { get; set; }
        [BindProperty]
        public string ParticipantId { get; set; }

        [BindProperty]
        public bool SendEmail { get; set; }


        [BindProperty]
        public bool SendSMS { get; set; }
        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ParticipantList = await _context.ParticipantLists.Include(x => x.Course).ThenInclude(x => x.CourseCategory).Include(x => x.User).FirstOrDefaultAsync(m => m.Id == id);

            if (ParticipantList == null)
            {
                return NotFound();
            }
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
                    if (user.Role == "Participant")
                    {
                        var sendmailparticipants = await _email.SendEmailWithResult(user.FullnameX, user.Email, "", "", $"NIPSS PARTICIPANT REGISTRATION", messagedetails);


                        if (sendmailparticipants == true)
                        {
                            user.EmailSent = true;
                        }
                        await _userManager.UpdateAsync(user);

                    }
                    else
                    {
                        var sendmail = await _email.SendEmailWithResult(user.FirstName, user.Email, "", "", $"NIPSS REGISTRATION", messagedetails);

                        if (sendmail == true)
                        {
                            user.EmailSent = true;
                        }
                        await _userManager.UpdateAsync(user);
                    }
                }
                return RedirectToPage("./RegistrationStatus", new { id = ParticipantId });

            }
            catch (Exception ex)
            {

                return Page();
            }
        }

    }
}
