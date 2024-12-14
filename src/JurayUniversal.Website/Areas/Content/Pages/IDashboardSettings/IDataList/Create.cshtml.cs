using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;

namespace JurayUniversal.Website.Areas.Content.Pages.IDashboardSettings.IDataList
{
    public class CreateModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public CreateModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["DashboardDataId"] = new SelectList(_context.DashboardDatas, "Id", "Title");

            ViewData["CompanyProgramId"] = new SelectList(_context.CompanyProgramCategories, "Id", "Title");

            return Page();
        }

        [BindProperty]
        public DashboardDataList DashboardDataList { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            

            _context.DashboardDataLists.Add(DashboardDataList);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
