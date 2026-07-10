using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisProjects.Pages.Projects
{
    [Authorize]
    public class CollaboratorsModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public CollaboratorsModel(DashboardDbContext context)
        {
            _context = context;
        }

        public MisProject MisProject { get; set; }
        public List<MisProjectUser> Collaborators { get; set; }
        public List<Profile> AvailableUsers { get; set; }

        public async Task<IActionResult> OnGetAsync(long id)
        {
            MisProject = await _context.MisProjects.FirstOrDefaultAsync(m => m.Id == id);

            if (MisProject == null)
            {
                return NotFound();
            }

            Collaborators = await _context.MisProjectUsers
                .Include(x => x.User)
                .Where(x => x.MisProjectId == id)
                .ToListAsync();

            var currentCollaboratorIds = Collaborators.Select(x => x.UserId).ToList();

            // Fetch users not already in this project
            AvailableUsers = await _context.Users
                .Where(x => !currentCollaboratorIds.Contains(x.Id))
                .Take(100) // limit for UI performance
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAddCollaboratorAsync(long projectId, string userId, string role)
        {
            if (string.IsNullOrEmpty(userId))
            {
                TempData["error"] = "Select a user to add.";
                return RedirectToPage(new { id = projectId });
            }

            var existing = await _context.MisProjectUsers
                .FirstOrDefaultAsync(x => x.MisProjectId == projectId && x.UserId == userId);
                
            if (existing == null)
            {
                var collab = new MisProjectUser
                {
                    MisProjectId = projectId,
                    UserId = userId,
                    Role = role ?? "Collaborator"
                };
                
                _context.MisProjectUsers.Add(collab);
                await _context.SaveChangesAsync();
                TempData["success"] = "Collaborator added successfully.";
            }

            return RedirectToPage(new { id = projectId });
        }
        
        public async Task<IActionResult> OnPostRemoveCollaboratorAsync(long collabId)
        {
            var collab = await _context.MisProjectUsers.FindAsync(collabId);
            if (collab != null)
            {
                long projectId = collab.MisProjectId;
                _context.MisProjectUsers.Remove(collab);
                await _context.SaveChangesAsync();
                TempData["success"] = "Collaborator removed.";
                return RedirectToPage(new { id = projectId });
            }
            return RedirectToPage("../Index");
        }
    }
}
