using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

using JurayUniversal.Domain.Models;
using JurayUniversal.Website.Areas.Admin.Pages.UserManagement;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.Admin.Pages.IProposalPage
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Manager")]

    public class CreateModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public CreateModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public  async Task<IActionResult> OnGetAsync()
        {
            var UserDatasList = new List<ProfileDto>();
            var UserDatas = await _userManager.Users.ToListAsync();

            var excludedRoles = new List<string> { "mSuperAdmin", "SubAdmin" };

            foreach (var user in UserDatas)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                if (userRoles.All(roleName => !excludedRoles.Contains(roleName)))
                {
                    var profile = new ProfileDto
                    {
                        Fullname = user.FirstName + " " + user.MiddleName + " " + user.LastName,
                        Phone = user.PhoneNumber,
                        Email = user.Email,
                        Status = user.UserStatus,
                        Id = user.Id,
                        Roles = string.Join(", ", userRoles) // Combine roles into a comma-separated string
                    };

                    UserDatasList.Add(profile);
                }
            }

            var users = UserDatasList.AsQueryable();

            ViewData["SubmittedById"] = new SelectList(users, "Id", "Fullname");
            return Page();
        }

        [BindProperty]
        public Proposal Proposal { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            Proposal.Date = DateTime.UtcNow.AddHours(1);

            _context.Proposals.Add(Proposal);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Details", new {id = Proposal.Id});
        }
    }
}
