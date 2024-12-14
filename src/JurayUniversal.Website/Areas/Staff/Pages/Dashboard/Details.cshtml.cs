
using JurayUniversal.Application.Services.AWS;
using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Web.Areas.Staff.Pages.Dashboard
{
    [Microsoft.AspNetCore.Authorization.Authorize]

    public class DetailsModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly IConfiguration _config;
        private readonly IStorageService _storageService;
        private readonly UserManager<Profile> _userManager;

        public DetailsModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, IConfiguration config, IStorageService storageService, UserManager<Profile> userManager)
        {
            _context = context;
            _config = config;
            _storageService = storageService;
            _userManager = userManager;
        }
        [BindProperty]
        public ProposalFile ProposalFile { get; set; }
        [BindProperty]
        public Proposal Proposal { get; set; }
        public List<ProposalFile> ProposalFiles { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Proposal = await _context.Proposals
                .Include(p => p.SubmittedBy).FirstOrDefaultAsync(m => m.Id == id);

            if (Proposal == null)
            {
                return NotFound();
            }

            ProposalFiles = await _context.ProposalFiles.Where(x => x.ProposalId == Proposal.Id).ToListAsync();
            return Page();
        }
        [BindProperty]
        public IFormFile? imagefile { get; set; }
        [BindProperty]
        public long PId { get; set; }
        [BindProperty]
        public string? NewReport { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var reportdata = await _context.Proposals.FirstOrDefaultAsync(x => x.Id == PId);
            if (NewReport != null)
            {
                reportdata.Note = reportdata.Note + "<hr style=\"border:2px solid white; margin:0;padding:0;\"> @updated on " + DateTime.UtcNow.AddHours(1) + " by "+user.GetFullName()+"<br>" + NewReport;

            }
            _context.Attach(reportdata).State = EntityState.Modified;
            await _context.SaveChangesAsync(); TempData["success"] = "Successful";

            return RedirectToPage("./Details", new { id = PId });
        }


 
    }

}
