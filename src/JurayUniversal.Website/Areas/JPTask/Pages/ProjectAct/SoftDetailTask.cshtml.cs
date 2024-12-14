using JurayUniversal.Application.Repository.NotifyRegister;
using JurayUniversal.Application.Services;
using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.JPTask.Pages.ProjectAct
{
       [Microsoft.AspNetCore.Authorization.Authorize]

    public class SoftDetailTaskModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;
        private readonly IEmailSendService _email;
        private readonly LoggerLibrary _logger;
        public SoftDetailTaskModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, UserManager<Profile> userManager, IEmailSendService email, LoggerLibrary logger)
        {
            _context = context;
            _userManager = userManager;
            _email = email;
            _logger = logger;
        }
        [BindProperty]
        public XProjectTask XProjectTask { get; set; }

        [BindProperty]
        public string? UserId { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            XProjectTask = await _context.XProjectTasks
                .Include(x => x.XProjectSection).ThenInclude(x => x.XProject)


                .Include(x => x.ApprovedBy)
                .Include(x => x.Files)
                .Include(x => x.TestedBy)
                .Include(x => x.User).FirstOrDefaultAsync(m => m.Id == id);

            if (XProjectTask == null)
            {
                return NotFound();
            }
            string userId = _userManager.GetUserId(HttpContext.User);
            UserId = userId;
            return Page();
        }

        public async Task<IActionResult> OnPostChatMessage(string message, long id)
        {
            string userId = _userManager.GetUserId(HttpContext.User);

            XProjectMessage msg = new XProjectMessage();
            msg.Message = message;
            msg.XProjectId = id;
            msg.SenderId = userId;
            msg.CreateDate = DateTime.UtcNow.AddHours(1);
            _context.XProjectMessages.Add(msg);

            await _context.SaveChangesAsync();
            TempData["success"] = "Successful";

            var XXProject = await _context.XProjects
               .Include(x => x.Sections)
        .ThenInclude(x => x.Tasks)

               .FirstOrDefaultAsync(m => m.Id == id);
            var listUsers = XXProject.Sections.SelectMany(x => x.Tasks)
                                    .SelectMany(t => new[] { t.UserId, t.TestedById, t.ApprovedById }).Distinct()
                                    .ToList();
            var configsettings = await _context.DataConfigs.FirstOrDefaultAsync();
            var settings = await _context.SuperSettings.FirstOrDefaultAsync();
            foreach (var item in listUsers)
            {
                try
                {

                    var xuser = await _userManager.FindByIdAsync(item);

                    string smsmessage = $"you have a new message on these Project {XXProject.Title}";
                    await _email.SendEmail(xuser.FirstName, xuser.Email, configsettings.CCmails, configsettings.BBmails, $"NEW MESSGAE ON TASK {XXProject.Title}", smsmessage);
                }
                catch (Exception ex)
                {
                    _logger.Log($" {settings.CompanyName} -  {ex.ToString()}");
                }
            }
            return RedirectToPage("./SoftDetails", new { id = id });
        }

        public async Task<IActionResult> OnPostUserReport(string message, long id)
        {
            string userId = _userManager.GetUserId(HttpContext.User);
            var xtask = await _context.XProjectTasks.FirstOrDefaultAsync(m => m.Id == id);
            xtask.UserTaskNote = xtask.UserTaskNote + "<li>" + message + "</li>";
            xtask.Status = XProjectTask.Status;
            xtask.UserDateFinished = DateTime.UtcNow.AddHours(1);
            _context.Attach(xtask).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            TempData["success"] = "Successful";

            var configsettings = await _context.DataConfigs.FirstOrDefaultAsync();
            var settings = await _context.SuperSettings.FirstOrDefaultAsync();
            //send to user
            try
            {
                string xmessage = "";
                var mainuser = await _userManager.FindByIdAsync(xtask.UserId);
                if (xtask.Status == Domain.Enum.Enum.ProjectTaskStatus.Done)
                {
                    xmessage = $"Update Task ({xtask.Title}) to status: {xtask.Status} on {xtask.FinishDate}. Kindly Check your dashboard for more details.";

                }
                else
                {
                    xmessage = $"Update Task ({xtask.Title}) to status: {xtask.Status}. Kindly Check your dashboard for more details.";

                }
                await _email.SendEmail(mainuser.FirstName, mainuser.Email, configsettings.CCmails, configsettings.BBmails, $"TASK {xtask.Title}", xmessage);
            }
            catch (Exception ex)
            {
                _logger.Log($" {settings.CompanyName} -  {ex.ToString()}");
            }

            return RedirectToPage("./SoftDetailTask", new { id = id });
        }

        public async Task<IActionResult> OnPostTesterReport(string message, long id)
        {
            string userId = _userManager.GetUserId(HttpContext.User);
            var xtask = await _context.XProjectTasks.FirstOrDefaultAsync(m => m.Id == id);
            xtask.TesterTaskNote = xtask.TesterTaskNote + "<li>" + message + "</li>";
            xtask.TestStatus = XProjectTask.TestStatus;
            xtask.TesterDateFinished = DateTime.UtcNow.AddHours(1);
            _context.Attach(xtask).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            TempData["success"] = "Successful";

            var configsettings = await _context.DataConfigs.FirstOrDefaultAsync();
            var settings = await _context.SuperSettings.FirstOrDefaultAsync();
            //send to user
            try
            {
                string xmessage = "";
                var mainuser = await _userManager.FindByIdAsync(xtask.TestedById);
                if (xtask.Status == Domain.Enum.Enum.ProjectTaskStatus.Done)
                {
                    xmessage = $"Update Testing Task ({xtask.Title}) to status: {xtask.Status} on {xtask.FinishDate}. Kindly Check your dashboard for more details.";

                }
                else
                {
                    xmessage = $"Update Testing Task ({xtask.Title}) to status: {xtask.Status}. Kindly Check your dashboard for more details.";

                }
                await _email.SendEmail(mainuser.FirstName, mainuser.Email, configsettings.CCmails, configsettings.BBmails, $"TASK {xtask.Title}", xmessage);
            }
            catch (Exception ex)
            {
                _logger.Log($" {settings.CompanyName} -  {ex.ToString()}");
            }
            return RedirectToPage("./SoftDetailTask", new { id = id });
        }
        public async Task<IActionResult> OnPostApprovalReport(string message, long id, int RateUser, int RateTester)
        {
            string userId = _userManager.GetUserId(HttpContext.User);
            var xtask = await _context.XProjectTasks.FirstOrDefaultAsync(m => m.Id == id);
            xtask.ApproveTaskNote = xtask.ApproveTaskNote + "<li>" + message + "</li>";
            xtask.ApproveStatus = XProjectTask.ApproveStatus;
            xtask.RateTester = RateTester;
            xtask.RateUser = RateUser;
            xtask.ApprovedDateFinished = DateTime.UtcNow.AddHours(1);
            _context.Attach(xtask).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            TempData["success"] = "Successful";
            var configsettings = await _context.DataConfigs.FirstOrDefaultAsync();
            var settings = await _context.SuperSettings.FirstOrDefaultAsync();
            //send to user
            try
            {
                string xmessage = "";
                var mainuser = await _userManager.FindByIdAsync(xtask.ApprovedById);
                if (xtask.Status == Domain.Enum.Enum.ProjectTaskStatus.Done)
                {
                    xmessage = $"Update Approval of Task ({xtask.Title}) to status: {xtask.Status} on {xtask.FinishDate}. Kindly Check your dashboard for more details.";

                }
                else
                {
                    xmessage = $"Update Approval of Task ({xtask.Title}) to status: {xtask.Status}. Kindly Check your dashboard for more details.";

                }
                await _email.SendEmail(mainuser.FirstName, mainuser.Email, configsettings.CCmails, configsettings.BBmails, $"TASK {xtask.Title}", xmessage);
            }
            catch (Exception ex)
            {
                _logger.Log($" {settings.CompanyName} -  {ex.ToString()}");
            }
            return RedirectToPage("./SoftDetailTask", new { id = id });
        }
        public async Task<IActionResult> OnPostCloseTask(bool close, long id)
        {
            string userId = _userManager.GetUserId(HttpContext.User);
            var xtask = await _context.XProjectTasks.FirstOrDefaultAsync(m => m.Id == id);
            if (xtask.CloseTask == false)
            {
                xtask.CloseTask = true;

            }
            else
            {
                xtask.CloseTask = false;


            }
            _context.Attach(xtask).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            TempData["success"] = "Successful";

            return RedirectToPage("./SoftDetailTask", new { id = id });
        }
    }
}
