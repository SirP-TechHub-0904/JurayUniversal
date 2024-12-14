using JurayUniversal.Domain.Models;
using JurayUniversal.Domain.Models.NIPSS;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.NIPSSADMIN.Pages.Participants
{
         public class StudyGroupListModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public StudyGroupListModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

      
        public List<StudyGroupCategory> StudyGroupCategory { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            
            if (id == null)
            {
                TempData["error"] = "Unable to fetch data";
                return RedirectToPage("/Dashboard/Index");
            }

            StudyGroupCategory = await _context.StudyGroupCategory.Include(x => x.StudyGroups)
                .ThenInclude(x => x.User)
                .Where(m => m.CourseId == id).ToListAsync();


            return Page();
        }


    }

}
