using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisGrants.Pages.Dashboard
{
    [Authorize(Policy = "MisGrantsEditor")]
    public class ManageModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public ManageModel(DashboardDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public MisGrant MisGrant { get; set; }

        public Microsoft.AspNetCore.Mvc.Rendering.SelectList DonorsList { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            var donors = await _context.MisDonors.ToListAsync();
            DonorsList = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(donors, "Id", "Name");

            if (id.HasValue)
            {
                MisGrant = await _context.MisGrants.FirstOrDefaultAsync(m => m.Id == id.Value);
                if (MisGrant == null)
                {
                    return NotFound();
                }
            }
            else
            {
                MisGrant = new MisGrant { IsActive = true, StartDate = System.DateTime.UtcNow.Date };
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // if (!ModelState.IsValid)
            // {
            //     var donors = await _context.MisDonors.ToListAsync();
            //     DonorsList = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(donors, "Id", "Name");
            //     return Page();
            // }

            if (MisGrant.Id == 0)
            {
                MisGrant.CreatedAt = System.DateTime.UtcNow;
                _context.MisGrants.Add(MisGrant);
                TempData["success"] = "Grant successfully created.";
            }
            else
            {
                var entity = await _context.MisGrants.FindAsync(MisGrant.Id);
                if (entity == null) return NotFound();

                entity.Title = MisGrant.Title;
                entity.MisDonorId = MisGrant.MisDonorId;
                entity.TotalAwardAmount = MisGrant.TotalAwardAmount;
                entity.Currency = MisGrant.Currency;
                entity.StartDate = MisGrant.StartDate;
                entity.EndDate = MisGrant.EndDate;
                entity.IsActive = MisGrant.IsActive;

                _context.Attach(entity).State = EntityState.Modified;
                TempData["success"] = "Grant successfully updated.";
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
