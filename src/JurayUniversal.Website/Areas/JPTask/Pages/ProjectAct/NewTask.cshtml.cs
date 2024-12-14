using JurayUniversal.Application.Repository.NotifyRegister;
using JurayUniversal.Application.Services;
using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.JPTask.Pages.ProjectAct
{
    [Microsoft.AspNetCore.Authorization.Authorize]

    public class NewTaskModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly IEmailSendService _email;
        private readonly UserManager<Profile> _userManager;
        private readonly LoggerLibrary _logger;

        public NewTaskModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, IEmailSendService email, UserManager<Profile> userManager, LoggerLibrary logger)
        {
            _context = context;
            _email = email;
            _userManager = userManager;
            _logger = logger;
        }
        [BindProperty]
        public long ProjectSectionId { get; set; }
        public async Task<IActionResult> OnGet(long id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var xXProject = await _context.XProjectSections.FirstOrDefaultAsync(m => m.Id == id);

            if (xXProject == null)
            {
                return NotFound();
            }
            ProjectSectionId = xXProject.Id;
             ViewData["ApprovedById"] = new SelectList(_context.Users.Where(x => x.UserStatus == Domain.Enum.Enum.UserStatus.Active), "Id", "Email");
            ViewData["TestedById"] = new SelectList(_context.Users.Where(x => x.UserStatus == Domain.Enum.Enum.UserStatus.Active), "Id", "Email");
            ViewData["UserId"] = new SelectList(_context.Users.Where(x => x.UserStatus == Domain.Enum.Enum.UserStatus.Active), "Id", "Email");
            return Page();
        }

        [BindProperty]
        public XProjectTask XProjectTask { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            
            _context.XProjectTasks.Add(XProjectTask);
            await _context.SaveChangesAsync();
            var configsettings = await _context.DataConfigs.FirstOrDefaultAsync();
            var settings = await _context.SuperSettings.FirstOrDefaultAsync();
            //send emails
            //send to user
            try { 
            var mainuser = await _userManager.FindByIdAsync(XProjectTask.UserId);
            
            string message = $"A New Task ({XProjectTask.Title}) has been create to start on {XProjectTask.StartDate} and finish on {XProjectTask.FinishDate}. Kindly Check your dashboard for more details.";
            await _email.SendEmail(mainuser.FirstName, mainuser.Email, configsettings.CCmails, configsettings.BBmails, $"NEW TASK {XProjectTask.Title}", message);
            }catch(Exception ex)
            { 
                _logger.Log($" {settings.CompanyName} -  {ex.ToString()}");
            }
            //send to tester
            try
            {
                var testuser = await _userManager.FindByIdAsync(XProjectTask.TestedById);

                string message = $"A Testing Task ({XProjectTask.Title}) has been create to start on {XProjectTask.TestDate} and finish on {XProjectTask.TesterDateFinished}. Kindly Check your dashboard for more details.";
                await _email.SendEmail(testuser.FirstName, testuser.Email, configsettings.CCmails, configsettings.BBmails, $"NEW TESTING TASK {XProjectTask.Title}", message);
            }
            catch (Exception ex)
            {
                _logger.Log($" {settings.CompanyName} -  {ex.ToString()}");
            }
            //send to tester
            try
            {
                var approveruser = await _userManager.FindByIdAsync(XProjectTask.ApprovedById);

                string message = $"A Task ({XProjectTask.Title}) you will Approve has been create to start on {XProjectTask.StartDate} and finish on {XProjectTask.FinishDate}. " +
                    $"<br>Test start on {XProjectTask.TestDate} and finish on {XProjectTask.TesterDateFinished}. Kindly Check your dashboard for more details.";
                await _email.SendEmail(approveruser.FirstName, approveruser.Email, configsettings.CCmails, configsettings.BBmails, $"NEW TESTING TASK {XProjectTask.Title}", message);
            }
            catch (Exception ex)
            {
                _logger.Log($" {settings.CompanyName} -  {ex.ToString()}");
            }



            var prj = await _context.XProjectSections.FirstOrDefaultAsync(x=>x.Id == XProjectTask.XProjectSectionId);


            return RedirectToPage("./Details", new {id = prj.XProjectId});
        }
    }
}
