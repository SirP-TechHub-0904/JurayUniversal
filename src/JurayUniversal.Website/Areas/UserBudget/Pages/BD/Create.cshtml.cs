using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Identity;
using System.Web.Mvc;

namespace JurayUniversal.Website.Areas.UserBudget.Pages.BD
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public CreateModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult OnGet()
        {
             return Page();
        }

        [BindProperty]
        public BudgetMainCategory BudgetMainCategory { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            string uid = _userManager.GetUserId(HttpContext.User);
            BudgetMainCategory.Date = DateTime.UtcNow.AddHours(1);
            BudgetMainCategory.LastUpdatedDate = DateTime.UtcNow.AddHours(1);
            BudgetMainCategory.UserId = uid;
            _context.BudgetMainCategories.Add(BudgetMainCategory);
            await _context.SaveChangesAsync();
            TempData["success"] = "success";
            return RedirectToPage("./Index");
        }
    }
}
