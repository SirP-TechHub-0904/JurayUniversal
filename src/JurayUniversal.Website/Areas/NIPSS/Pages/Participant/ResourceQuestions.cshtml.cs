using JurayUniversal.Domain.Models;
using JurayUniversal.Domain.Models.NIPSS;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.NIPSS.Pages.Participant
{
    public class ResourceQuestionsModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public ResourceQuestionsModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public Moderator Moderator { get; set; }
        public async Task<IActionResult> OnGet(long? mid)
        {
            MID = mid;
             

            Moderator = await _context.Moderators.Include(x=>x.User).Include(x=>x.TimeTable).FirstOrDefaultAsync(x=>x.Id == mid);
            if(Moderator == null)
            {
                return NotFound();
            }

            QuestionAndAnswers = await _context.QuestionAndAnswers.Include(x=>x.User).Where(x=>x.ModeratorId == mid).ToListAsync();
            return Page();
        }

        public string UID { get; set; }
        public long? MID { get; set; }

        [BindProperty]
        public QuestionAndAnswer QuestionAndAnswer { get; set; }
        public List<QuestionAndAnswer> QuestionAndAnswers { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            QuestionAndAnswer.UserId = user.Id;
            QuestionAndAnswer.Date = DateTime.UtcNow.AddHours(1);
            _context.QuestionAndAnswers.Add(QuestionAndAnswer);
            await _context.SaveChangesAsync();

            return RedirectToPage("./ResourceQuestions", new {mid = QuestionAndAnswer.ModeratorId});
        }
    }
} 
