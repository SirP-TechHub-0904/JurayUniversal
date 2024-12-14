using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;

namespace JurayUniversal.Website.Areas.Admin.Pages.XProjectPage.XM
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
        ViewData["ReceiverId"] = new SelectList(_context.Users, "Id", "Id");
        ViewData["SenderId"] = new SelectList(_context.Users, "Id", "Id");
        ViewData["XProjectId"] = new SelectList(_context.XProjects, "Id", "Description");
            return Page();
        }

        [BindProperty]
        public XProjectMessage XProjectMessage { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.XProjectMessages.Add(XProjectMessage);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
