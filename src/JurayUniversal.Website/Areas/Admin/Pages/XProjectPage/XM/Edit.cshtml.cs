using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;

namespace JurayUniversal.Website.Areas.Admin.Pages.XProjectPage.XM
{
    public class EditModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public EditModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public XProjectMessage XProjectMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            XProjectMessage = await _context.XProjectMessages
                .Include(x => x.Receiver)
                .Include(x => x.Sender)
                .Include(x => x.XProject).FirstOrDefaultAsync(m => m.Id == id);

            if (XProjectMessage == null)
            {
                return NotFound();
            }
           ViewData["ReceiverId"] = new SelectList(_context.Users, "Id", "Id");
           ViewData["SenderId"] = new SelectList(_context.Users, "Id", "Id");
           ViewData["XProjectId"] = new SelectList(_context.XProjects, "Id", "Description");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(XProjectMessage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!XProjectMessageExists(XProjectMessage.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool XProjectMessageExists(long id)
        {
            return _context.XProjectMessages.Any(e => e.Id == id);
        }
    }
}
