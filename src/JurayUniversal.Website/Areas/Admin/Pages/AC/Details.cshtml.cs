using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.Admin.Pages.AC
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Manager,Admin")]

    public class DetailsModel : PageModel
    {
        private readonly UserManager<Profile> _userManager;
        private readonly SignInManager<Profile> _signInManager;
        private readonly ILogger<DetailsModel> _logger;
                private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public DetailsModel(SignInManager<Profile> signInManager,
            ILogger<DetailsModel> logger,
            UserManager<Profile> userManager,
            Persistence.EF.SQL.DashboardDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }
        public Profile UserDatas { get; set; } 
        public IList<AdditionalProfile> AdditionalProfile { get; set; } 
        public IList<ProfileCategory> ProfileCategories { get; set; }
        public UserCategory UserCategory { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UserDatas = await _userManager.FindByIdAsync(id);

            if (UserDatas == null)
            {
                return NotFound();
            }
            AdditionalProfile = await _context.AdditionalProfiles.Where(x=>x.ProfileId == UserDatas.Id).ToListAsync();
            ProfileCategories = await _context.ProfileCategories
                .Include(x=>x.ProfileCategoryLists).Where(x=>x.ProfileId == UserDatas.Id).ToListAsync();

            UserCategory = await _context.UserCategories.FirstOrDefaultAsync(x => x.Id == UserDatas.UserCategoryId);

            return Page();
        }
    }
}
