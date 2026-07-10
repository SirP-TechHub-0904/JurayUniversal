using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisVoting.Pages.Dashboard.Elections
{
    [Authorize] // Ideally we would have a specific policy for Admin
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public IndexModel(DashboardDbContext context)
        {
            _context = context;
        }

        public IList<MisElection> Elections { get; set; } = new List<MisElection>();

        public int TotalElections { get; set; }
        public int TotalVotesCast { get; set; }
        public int TotalAccreditedVoters { get; set; }

        // Chart Data
        public List<string> ElectionLabels { get; set; } = new List<string>();
        public List<int> ElectionVotesData { get; set; } = new List<int>();

        public List<string> StatusLabels { get; set; } = new List<string> { "Upcoming", "Active", "Completed" };
        public List<int> StatusData { get; set; } = new List<int>();

        public async Task OnGetAsync()
        {
            Elections = await _context.MisElections
                .Include(x => x.MisElectionVoters)
                .Include(x => x.MisVoteCategories)
                    .ThenInclude(c => c.MisUserVotes)
                .OrderByDescending(x => x.StartDate)
                .AsNoTracking()
                .ToListAsync();

            TotalElections = Elections.Count;
            TotalVotesCast = Elections.Sum(e => e.MisVoteCategories.Sum(c => c.MisUserVotes.Count));
            TotalAccreditedVoters = Elections.Sum(e => e.MisElectionVoters.Count(v => v.IsAccredited));

            // Chart Data for Top 5 recent elections turnout
            var topElections = Elections.Take(5).ToList();
            foreach (var election in topElections)
            {
                ElectionLabels.Add(election.Title.Length > 15 ? election.Title.Substring(0, 15) + "..." : election.Title);
                ElectionVotesData.Add(election.MisVoteCategories.Sum(c => c.MisUserVotes.Count));
            }

            // Status distribution
            StatusData = new List<int>
            {
                Elections.Count(e => e.VotingStatus == Domain.Enum.Enum.MisVotingStatus.Upcoming),
                Elections.Count(e => e.VotingStatus == Domain.Enum.Enum.MisVotingStatus.Active),
                Elections.Count(e => e.VotingStatus == Domain.Enum.Enum.MisVotingStatus.Completed)
            };
        }
    }
}

