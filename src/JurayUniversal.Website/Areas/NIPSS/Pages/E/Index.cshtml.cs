using JurayUniversal.Application.Services.AWS;
using JurayUniversal.Domain.Models;
using JurayUniversal.Domain.Models.NIPSS;
using JurayUniversal.Website.Areas.NIPSSADMIN.Pages.ManageCourse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.NIPSS.Pages.E
{
 
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly IConfiguration _config;
        private readonly IStorageService _storageService;
        public IndexModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, IConfiguration config, IStorageService storageService)
        {
            _context = context;
            _config = config;
            _storageService = storageService;
        }

        public Evoting Evoting { get; set; }

        [BindProperty]
        public VoteCategory VoteCategory { get; set; }
        public SuperSetting SuperSetting { get; set; }

        public List<VoteCondidate> VoteCondidates { get; set; }
        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Evoting = await _context.Evotings
                .Include(v => v.Course)
                .ThenInclude(v => v.CourseCategory)
                .Include(v => v.VoteCategories)

                .FirstOrDefaultAsync(m => m.Id == id);

            if (Evoting == null)
            {
                return NotFound();
            }
            SuperSetting = await _context.SuperSettings.FirstOrDefaultAsync();


            return Page();
        }

    }

}
