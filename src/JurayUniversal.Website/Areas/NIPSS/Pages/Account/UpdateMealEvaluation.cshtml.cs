using JurayUniversal.Application.Services.AWS;
using JurayUniversal.Domain.Models.NIPSS;
using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.NIPSS.Pages.Account
{
    [Microsoft.AspNetCore.Authorization.Authorize]

    public class UpdateMealEvaluationModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;
        private readonly IConfiguration _config;
        private readonly IStorageService _storageService;
        public UpdateMealEvaluationModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, UserManager<Profile> userManager, IConfiguration config, IStorageService storageService)
        {
            _context = context;
            _userManager = userManager;
            _config = config;
            _storageService = storageService;
        }

        public IList<NutritionCategoryList> NutritionCategoryList { get; set; }
        [BindProperty]
        public Profile UserDatas { get; set; }


        public string UserId { get; set; }
        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (id != null)
            {
                if (User.Identity.IsAuthenticated && User.IsInRole("Admin"))
                {
                    UserDatas = await _userManager.FindByIdAsync(id);
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                var user = await _userManager.GetUserAsync(User);
                UserDatas = await _userManager.FindByIdAsync(user.Id);
            }

            //NutritionCategoryList = await _context.NutritionCategoryLists
            //    .Include(x => x.NutritionCategory)
            //    .Include(x => x.UserNutritionEvaluation) 
            //    //&&
            //    //x.NutritionCategoryList.Any(nc =>
            //    //    nc.UserNutritionEvaluation != null &&
            //    //    nc.UserNutritionEvaluation.UserId == id)
            //    )
            //    .ToListAsync();

            return Page();
        }
    }
}
