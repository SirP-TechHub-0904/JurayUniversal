using JurayUniversal.Domain.Models.NIPSS;
using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.NIPSSADMIN.Pages.NutriValution
{
     [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Admin,Evaluator")]

    public class UserEvaluationResultModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public UserEvaluationResultModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<NutritionCategory> NutritionCategory { get; set; }
        [BindProperty]
        public IList<NutritionCategory> ParticipantNutritionCategory { get; set; }

        [BindProperty]
        public IList<UserNutritionEvaluation> UserNutritionEvaluation { get; set; }


        [BindProperty]
        public Profile UserDatas { get; set; }
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


             
              ParticipantNutritionCategory = await _context.NutritionCategories
                //.Include(x => x.NutritionCategoryList)
                //.Include(x => x.UserNutritionEvaluation).ThenInclude(x => x.NutritionCategoryList)
                //.Where(x => x.Disable == false)
                //.Where(x => x.UserNutritionEvaluation.Any(e => e.UserId == UserDatas.Id))
                .ToListAsync();

            UserNutritionEvaluation = await _context.UserNutritionEvaluations
                .Include(x => x.NutritionCategoryList)
                .Where(x => x.UserId == UserDatas.Id)
                .ToListAsync();

            return Page();
        }


 
    }
}
