using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JurayUniversal.MisCentral.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayUniversal.MisCentral.Areas.Administrator.Pages.Leads
{
    public class IndexModel : PageModel
    {
        private readonly IProspectiveLeadService _leadService;

        public IndexModel(IProspectiveLeadService leadService)
        {
            _leadService = leadService;
        }

        public List<ProspectiveLead> Leads { get; set; } = new();
        public int TotalCount => Leads.Count;
        public int NewCount => Leads.Count(l => l.Status == "New");
        public int DemoCount => Leads.Count(l => l.Status == "Demo Scheduled");
        public int ConvertedCount => Leads.Count(l => l.Status == "Won/Converted");

        [BindProperty(SupportsGet = true)]
        public string? FilterStatus { get; set; }

        public async Task OnGetAsync()
        {
            var all = await _leadService.GetAllLeadsAsync();
            if (!string.IsNullOrEmpty(FilterStatus) && FilterStatus != "All")
            {
                Leads = all.Where(l => l.Status == FilterStatus).ToList();
            }
            else
            {
                Leads = all;
            }
        }

        public async Task<IActionResult> OnPostUpdateStatusAsync(string id, string status, string? adminNotes)
        {
            await _leadService.UpdateStatusAsync(id, status, adminNotes);
            return RedirectToPage(new { FilterStatus });
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            await _leadService.DeleteLeadAsync(id);
            return RedirectToPage(new { FilterStatus });
        }
    }
}
