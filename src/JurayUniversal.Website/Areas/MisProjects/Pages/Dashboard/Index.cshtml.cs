using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisProjects.Pages.Dashboard
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public IndexModel(DashboardDbContext context)
        {
            _context = context;
        }

        public int TotalProjects { get; set; }
        public int ActiveSprints { get; set; }
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }

        public List<string> StatusLabels { get; set; } = new List<string> { "To Do", "In Progress", "Review", "Done" };
        public List<int> StatusData { get; set; } = new List<int>();

        public async Task OnGetAsync()
        {
            TotalProjects = await _context.MisProjects.CountAsync();
            ActiveSprints = await _context.MisProjectSprints.CountAsync(s => s.Status == Domain.Enum.Enum.MisSprintStatus.Active);
            
            var allTasks = await _context.MisProjectTasks.AsNoTracking().ToListAsync();
            TotalTasks = allTasks.Count;
            CompletedTasks = allTasks.Count(t => t.Status == Domain.Enum.Enum.MisTaskStatus.Done);

            StatusData = new List<int>
            {
                allTasks.Count(t => t.Status == Domain.Enum.Enum.MisTaskStatus.ToDo),
                allTasks.Count(t => t.Status == Domain.Enum.Enum.MisTaskStatus.InProgress),
                allTasks.Count(t => t.Status == Domain.Enum.Enum.MisTaskStatus.Review),
                allTasks.Count(t => t.Status == Domain.Enum.Enum.MisTaskStatus.Done)
            };
        }
    }
}
