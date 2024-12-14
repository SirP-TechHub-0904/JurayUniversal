using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.Content.Pages.CM
{
    public class CreateModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public CreateModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGet(long id)
        {
            ViewData["CompanyProgramCategoryId"] = new SelectList(_context.CompanyProgramCategories, "Id", "Title");
            CompanyProgramCategory = await _context.CompanyProgramCategories.FirstOrDefaultAsync(x => x.Id == id);
            return Page();
        }

        [BindProperty]
        public CompanyProgram CompanyProgram { get; set; }
        public CompanyProgramCategory CompanyProgramCategory { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {


            _context.CompanyPrograms.Add(CompanyProgram);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index", new { id = CompanyProgram.CompanyProgramCategoryId });
        }
    }
}
