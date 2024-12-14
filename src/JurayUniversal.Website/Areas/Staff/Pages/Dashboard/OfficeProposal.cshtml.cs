using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Web.Areas.Staff.Pages.Dashboard
{
    [Microsoft.AspNetCore.Authorization.Authorize]

    public class OfficeProposalModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public OfficeProposalModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public IList<Proposal> Proposal { get; set; }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            var superrole = await _userManager.IsInRoleAsync(user, "mSuperAdmin");
            var adminrole = await _userManager.IsInRoleAsync(user, "Admin");
            var staffrole = await _userManager.IsInRoleAsync(user, "Manager");

            if (superrole.Equals(true) || adminrole.Equals(true) || staffrole.Equals(true))
            {
                Proposal = await _context.Proposals
                    .Include(p => p.SubmittedBy).ToListAsync();
            }
            else 
            {
                Proposal = await _context.Proposals
                    .Include(p => p.SubmittedBy).Where(x=>x.SubmittedById == user.Id).ToListAsync();
            }
            
        }

    }
}
