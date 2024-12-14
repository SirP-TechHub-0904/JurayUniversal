using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Pages.Shared.ViewComponents
{
    public class XUserImagesListViewComponent : ViewComponent
    {
        private readonly DashboardDbContext _context;

        public XUserImagesListViewComponent(
            DashboardDbContext context)
        {
            _context = context;
        }

        public string UserInfo { get; set; }

        public async Task<IViewComponentResult> InvokeAsync(long id)
        {
            var XProject = await _context.XProjects
      .Include(x => x.Sections)
          .ThenInclude(x => x.Tasks)
              .ThenInclude(t => t.User) // Include User profile
          .Include(x => x.Sections)
              .ThenInclude(x => x.Tasks)
                  .ThenInclude(t => t.TestedBy) // Include TestedBy profile
          .Include(x => x.Sections)
              .ThenInclude(x => x.Tasks)
                  .ThenInclude(t => t.ApprovedBy) // Include ApprovedBy profile
      .FirstOrDefaultAsync(m => m.Id == id);

            var listUsers = XProject.Sections.SelectMany(x => x.Tasks)
                .SelectMany(t => new[] { t.User, t.TestedBy, t.ApprovedBy })
                .Where(profile => profile != null) // Exclude null profiles, adjust as needed
                .Distinct()
                .ToList();


            return View(listUsers);
        }
    }
}
