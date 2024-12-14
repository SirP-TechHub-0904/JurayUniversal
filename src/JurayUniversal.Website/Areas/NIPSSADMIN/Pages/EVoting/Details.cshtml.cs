using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using JurayUniversal.Domain.Models.NIPSS;
using JurayUniversal.Persistence.EF.SQL;
using JurayUniversal.Website.Areas.NIPSSADMIN.Pages.ManageCourse;
using Microsoft.AspNetCore.Mvc.Rendering;
using JurayUniversal.Application.Services.AWS;
using JurayUniversal.Application.Dtos.AwsDtos;

namespace JurayUniversal.Website.Areas.NIPSSADMIN.Pages.EVoting
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Admin")]

    public class DetailsModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly IConfiguration _config;
        private readonly IStorageService _storageService;
        public DetailsModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, IConfiguration config, IStorageService storageService)
        {
            _context = context;
            _config = config;
            _storageService = storageService;
        }

        public Evoting Evoting { get; set; }

        [BindProperty]
        public VoteCategory VoteCategory { get; set; }

        public List<VoteCondidate> VoteCondidates { get; set; }
        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Evoting = await _context.Evotings
                .Include(v => v.Course)
                .ThenInclude(v => v.CourseCategory)
                .Include(v => v.VoteCategories) 

                .FirstOrDefaultAsync(m => m.Id == id);

            if (Evoting == null)
            {
                return NotFound();
            }

            //
            VoteCondidates = await _context.VoteCondidates
                .Include(x=>x.UserVote)
                .Include(x=>x.VoteCategory)
                .Where(x=>x.VoteCategory.EvotingId == Evoting.Id).ToListAsync();
            // Fetch data and map it to DTO
            var listsDS = _context.ParticipantLists
                .Include(x => x.User)

                .Where(x => x.CourseId == Evoting.CourseId)
                .OrderBy(x => x.User.LastName)
                .Select(x => new StaffManagerDto
                {
                    UserId = x.User.Id,
                    Fullname = x.User.FullnameX
                })
                .ToList();
            // Assign data to ViewData
            ViewData["CandidateId"] = new SelectList(listsDS, "UserId", "Fullname");
            return Page();
        }
        [BindProperty]
        public IFormFile? imagefile { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
           
            _context.VoteCategories.Add(VoteCategory);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Details", new { id = VoteCategory.EvotingId });
        }
    }
}
