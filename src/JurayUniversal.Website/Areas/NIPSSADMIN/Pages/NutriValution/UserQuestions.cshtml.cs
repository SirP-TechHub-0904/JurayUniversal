using JurayUniversal.Domain.Models;
using JurayUniversal.Domain.Models.NIPSS;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.NIPSSADMIN.Pages.NutriValution
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Admin,Evaluator")]

    public class UserQuestionsModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public UserQuestionsModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, UserManager<Profile> userManager)
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




            NutritionCategory = await _context.NutritionCategories

                .Include(x => x.NutritionCategoryList)

                .Where(x => x.Disable == false)
                .ToListAsync();
            //&& x.UserNutritionEvaluation.Where(x => x.UserId == UserDatas.Id)
            foreach (var category in NutritionCategory)
            {
                foreach (var item in category.NutritionCategoryList)
                {

                    var check = await _context.UserNutritionEvaluations.FirstOrDefaultAsync(x => x.UserId == UserDatas.Id && x.NutritionCategoryListId == item.Id && x.NutritionCategoryId == category.Id);
                    if (check == null)
                    {


                        UserNutritionEvaluation xnutri = new UserNutritionEvaluation();
                        xnutri.UserId = UserDatas.Id;
                        xnutri.NutritionCategoryListId = item.Id;
                        xnutri.ResultType = item.ResultType;
                        xnutri.NutritionCategoryId = category.Id;
                        _context.UserNutritionEvaluations.Add(xnutri);

                    }
                    await _context.SaveChangesAsync();
                }


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


        public async Task<IActionResult> OnPostAsync()
        {
            // Get the current user
            //ParticipantNutritionCategory = await _context.NutritionCategories
            //   .Include(x => x.NutritionCategoryList)
            //   .Include(x => x.UserNutritionEvaluation).ThenInclude(x => x.NutritionCategoryList)
            //   .Where(x => x.Disable == false)
            //   .Where(x => x.UserNutritionEvaluation.Any(e => e.UserId == UserDatas.Id))
            //   .ToListAsync();

            //foreach (var list in ParticipantNutritionCategory)
            //{
            var nextevaluation = _context.UserNutritionEvaluations.Where(x => x.UserId == UserDatas.Id).ToList();
            // Process the form submission
            foreach (var item in nextevaluation)
            {
                var value = Request.Form[$"NutritionResult_{item.Id}"].ToString();
                var getitemtoupdate = await _context.UserNutritionEvaluations.FindAsync(item.Id);

                getitemtoupdate.Result = value;
                _context.Attach(getitemtoupdate).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }


            return RedirectToPage("./UserQuestions", new { id = UserDatas.Id }); // Redirect back to the same page after submission
        }

    }
}
