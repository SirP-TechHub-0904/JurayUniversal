using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;

namespace JurayUniversal.Website.Areas.SMS.Pages.Messages
{
    public class CreateModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public CreateModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public long Id { get; set; }
        public IActionResult OnGet(long id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Id = id;
            return Page();
        }

        [BindProperty]
        public SmsMessage SmsMessage { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.SmsMessages.Add(SmsMessage);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
