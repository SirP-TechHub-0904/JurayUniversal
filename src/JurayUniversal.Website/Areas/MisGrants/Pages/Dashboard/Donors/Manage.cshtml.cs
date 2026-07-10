using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisGrants.Pages.Dashboard.Donors
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
        public MisDonor Donor { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id.HasValue)
            {
                Donor = await _context.MisDonors.FirstOrDefaultAsync(d => d.Id == id.Value);
                if (Donor == null) return NotFound();
            }
            else
            {
                Donor = new MisDonor();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            if (Donor.Id == 0)
            {
                Donor.CreatedAt = System.DateTime.UtcNow;
                _context.MisDonors.Add(Donor);
                TempData["success"] = "Donor created successfully.";
            }
            else
            {
                var entity = await _context.MisDonors.FindAsync(Donor.Id);
                if (entity == null) return NotFound();

                entity.Name = Donor.Name;
                entity.Description = Donor.Description;
                entity.ContactEmail = Donor.ContactEmail;
                entity.ContactPhone = Donor.ContactPhone;
                entity.Address = Donor.Address;

                _context.Attach(entity).State = EntityState.Modified;
                TempData["success"] = "Donor updated successfully.";
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
