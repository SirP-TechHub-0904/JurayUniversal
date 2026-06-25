using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisReports.Pages.Dashboard.Schemas
{
    [Authorize(Policy = "MisReportsEditor")]
    public class CreateModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public CreateModel(DashboardDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public MisReportSchema Schema { get; set; }

        [BindProperty]
        public bool AssignToAllActiveUsers { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // if (!ModelState.IsValid)
            // {
            //     return Page();
            // }

            _context.MisReportSchemas.Add(Schema);
            await _context.SaveChangesAsync();

            if (AssignToAllActiveUsers)
            {
                var activeUsers = await _context.Users
                    .Where(u => u.UserStatus == JurayUniversal.Domain.Enum.Enum.UserStatus.Active)
                    .ToListAsync();
                    
                foreach (var user in activeUsers)
                {
                    _context.MisReportParticipants.Add(new MisReportParticipant
                    {
                        MisReportSchemaId = Schema.Id,
                        UserProfileId = user.Id
                    });
                }
                await _context.SaveChangesAsync();
            }

            TempData["success"] = "Report Schema created successfully.";
            return RedirectToPage("./Index");
        }
    }
}
