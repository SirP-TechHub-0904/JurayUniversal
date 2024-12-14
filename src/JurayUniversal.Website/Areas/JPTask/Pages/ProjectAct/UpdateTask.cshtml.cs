using JurayUniversal.Application.Repository.NotifyRegister;
using JurayUniversal.Application.Services;
using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.JPTask.Pages.ProjectAct
{
    [Microsoft.AspNetCore.Authorization.Authorize]

    public class UpdateTaskModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly IEmailSendService _email;
        private readonly UserManager<Profile> _userManager;
        private readonly LoggerLibrary _logger;
        public UpdateTaskModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, IEmailSendService email, UserManager<Profile> userManager, LoggerLibrary logger)
        {
            _context = context;
            _email = email;
            _userManager = userManager;
            _logger = logger;
        }

        [BindProperty]
        public XProjectTask XProjectTask { get; set; }

        [BindProperty]
        public string PreviouseUser { get; set; }

        [BindProperty]
        public string PreviouseTester { get; set; }

        [BindProperty]
        public string PreviouseApprover { get; set; }


        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            XProjectTask = await _context.XProjectTasks
                .Include(x => x.ApprovedBy)
                .Include(x => x.TestedBy)
                .Include(x => x.XProjectSection)
                .Include(x => x.User).FirstOrDefaultAsync(m => m.Id == id);

            if (XProjectTask == null)
            {
                return NotFound();
            }
            ViewData["ApprovedById"] = new SelectList(_context.Users.Where(x => x.UserStatus == Domain.Enum.Enum.UserStatus.Active), "Id", "Email");
            ViewData["TestedById"] = new SelectList(_context.Users.Where(x => x.UserStatus == Domain.Enum.Enum.UserStatus.Active), "Id", "Email");
            ViewData["UserId"] = new SelectList(_context.Users.Where(x => x.UserStatus == Domain.Enum.Enum.UserStatus.Active), "Id", "Email");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {


            _context.Attach(XProjectTask).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!XProjectTaskExists(XProjectTask.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            var configsettings = await _context.DataConfigs.FirstOrDefaultAsync();
            var settings = await _context.SuperSettings.FirstOrDefaultAsync();
            //send emails
            //send to user
            try
            {
                if (PreviouseUser != XProjectTask.UserId)
                {
                    var mainuser = await _userManager.FindByIdAsync(PreviouseUser);
                    string message = $"You have been removed from Task ({XProjectTask.Title})";
                    await _email.SendEmail(mainuser.FirstName, mainuser.Email, configsettings.CCmails, configsettings.BBmails, $"UPDATED TASK {XProjectTask.Title}", message);

                }
                else
                {
                    var mainuser = await _userManager.FindByIdAsync(XProjectTask.UserId);
                    string message = $"Task ({XProjectTask.Title}) has been update to start on {XProjectTask.StartDate} and finish on {XProjectTask.FinishDate}. Kindly Check your dashboard for more details.";
                    await _email.SendEmail(mainuser.FirstName, mainuser.Email, configsettings.CCmails, configsettings.BBmails, $"UPDATED TASK {XProjectTask.Title}", message);
                }
            }
            catch (Exception ex)
            {
                _logger.Log($" {settings.CompanyName} -  {ex.ToString()}");
            }
            //send to tester
            try
            {
                if (PreviouseTester != XProjectTask.TestedById)
                {
                    var mainuser = await _userManager.FindByIdAsync(PreviouseTester);
                    string message = $"You have been removed as tester from Task ({XProjectTask.Title})";
                    await _email.SendEmail(mainuser.FirstName, mainuser.Email, configsettings.CCmails, configsettings.BBmails, $"UPDATED TESTING TASK {XProjectTask.Title}", message);

                }
                else
                {
                    var testuser = await _userManager.FindByIdAsync(XProjectTask.TestedById);

                string message = $"Your Testing Task ({XProjectTask.Title}) has been update to start on {XProjectTask.TestDate} and finish on {XProjectTask.TesterDateFinished}. Kindly Check your dashboard for more details.";
                await _email.SendEmail(testuser.FirstName, testuser.Email, configsettings.CCmails, configsettings.BBmails, $"UPDATED TESTING TASK {XProjectTask.Title}", message);

                } }
            catch (Exception ex)
            {
                _logger.Log($" {settings.CompanyName} -  {ex.ToString()}");
            }
            //send to approveral
            try
            {
                if (PreviouseApprover != XProjectTask.ApprovedById)
                {
                    var mainuser = await _userManager.FindByIdAsync(PreviouseApprover);
                    string message = $"You have been removed as approval of Task ({XProjectTask.Title})";
                    await _email.SendEmail(mainuser.FirstName, mainuser.Email, configsettings.CCmails, configsettings.BBmails, $"UPDATED APPROVAL TASK {XProjectTask.Title}", message);

                }
                else
                {

                    var approveruser = await _userManager.FindByIdAsync(XProjectTask.ApprovedById);

                string message = $"Your Task ({XProjectTask.Title}) you will Approve has been update to start on {XProjectTask.StartDate} and finish on {XProjectTask.FinishDate}. " +
                    $"<br>Test start on {XProjectTask.TestDate} and finish on {XProjectTask.TesterDateFinished}. Kindly Check your dashboard for more details.";
                await _email.SendEmail(approveruser.FirstName, approveruser.Email, configsettings.CCmails, configsettings.BBmails, $"UPDATED APPROVAL TASK {XProjectTask.Title}", message);
                }
                }
            catch (Exception ex)
            {
                _logger.Log($" {settings.CompanyName} -  {ex.ToString()}");
            }

            var prj = await _context.XProjectSections.FirstOrDefaultAsync(x => x.Id == XProjectTask.XProjectSectionId);


            return RedirectToPage("./Details", new { id = prj.XProjectId });
        }

        private bool XProjectTaskExists(long id)
        {
            return _context.XProjectTasks.Any(e => e.Id == id);
        }
    }

}
