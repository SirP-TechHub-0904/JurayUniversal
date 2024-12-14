using JurayUniversal.Domain.Models.NIPSS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.NIPSSADMIN.Pages.EVP
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Admin")]

    public class IndexModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public IndexModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _context = context;
        }

        public IList<EvaluationQuestion> EvaluationQuestion { get; set; }

        public async Task OnGetAsync()
        {
            EvaluationQuestion = await _context.EvaluationQuestions.ToListAsync();
        }
    }
}
