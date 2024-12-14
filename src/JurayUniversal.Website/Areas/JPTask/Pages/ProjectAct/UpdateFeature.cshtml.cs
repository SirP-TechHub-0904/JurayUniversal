using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.JPTask.Pages.ProjectAct
{
    [Microsoft.AspNetCore.Authorization.Authorize]

    public class UpdateFeatureModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public UpdateFeatureModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public XProjectSection XProjectSection { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            XProjectSection = await _context.XProjectSections.FirstOrDefaultAsync(m => m.Id == id);

            if (XProjectSection == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {


            _context.Attach(XProjectSection).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                throw;

            }
            TempData["success"] = "Successfully";

            return RedirectToPage("./Details", new {id = XProjectSection.XProjectId});
        }

        private bool XProjectSectionExists(long id)
        {
            return _context.XProjectSections.Any(e => e.Id == id);
        }
    }

}
