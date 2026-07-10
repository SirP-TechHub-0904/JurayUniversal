using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisVoting.Pages.Dashboard.Elections
{
    public class CreateModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public CreateModel(DashboardDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public MisElection MisElection { get; set; } = new MisElection();

        public IActionResult OnGet()
        {
            var now = DateTime.UtcNow;
            MisElection.StartDate = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);
            MisElection.EndDate = MisElection.StartDate.AddDays(7);
            MisElection.ElectionType = Domain.Enum.Enum.MisElectionType.Open;
            MisElection.VotingStatus = Domain.Enum.Enum.MisVotingStatus.Upcoming;
            MisElection.ShowResults = true;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // if (!ModelState.IsValid)
            // {
            //     return Page();
            // }

            string code;
            do
            {
                code = Guid.NewGuid().ToString("N").Substring(0, 10).ToLower();
            } while (_context.MisElections.Any(e => e.ShortCode == code));

            MisElection.ShortCode = code;

            _context.MisElections.Add(MisElection);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Election created successfully!";
            return RedirectToPage("./Index");
        }
    }
}
