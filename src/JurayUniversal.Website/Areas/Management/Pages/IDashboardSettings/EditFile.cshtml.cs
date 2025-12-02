using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using JurayUniversal.Persistence.EF.SQL;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.Management.Pages.IDashboardSettings
{
    public class EditFileModel : PageModel
    {
        private readonly DashboardDbContext _context;

        public EditFileModel(DashboardDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)] public string Entity { get; set; }
        [BindProperty(SupportsGet = true)] public long Id { get; set; }
        [BindProperty(SupportsGet = true)] public string Property { get; set; }
        [BindProperty(SupportsGet = true)] public string Key { get; set; }
        [BindProperty(SupportsGet = true)] public string Url { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            bool ok = await UpdateEntityFile(Entity, Id, Property, Url, Key);
            if (ok)
                TempData["msg"] = "Updated successfully.";
            else
                TempData["msg"] = "Failed to update entry.";

            return RedirectToPage("AllFilesReconciliation");
        }

        // Reuse your reflection updater
        private async Task<bool> UpdateEntityFile(string entity, long id, string property, string newUrl, string key)
        {
            object obj = entity switch
            {
                "Blog" => _context.Blogs.FirstOrDefault(x => x.Id == id),
                "Product" => _context.Products.FirstOrDefault(x => x.Id == id),
                "ProductSample" => _context.ProductSamples.FirstOrDefault(x => x.Id == id),
                "PageSection" => _context.PageSections.FirstOrDefault(x => x.Id == id),
                "PageSectionList" => _context.PageSectionLists.FirstOrDefault(x => x.Id == id),
                "WebPage" => _context.WebPages.FirstOrDefault(x => x.Id == id),
                "Testimony" => _context.Testimonies.FirstOrDefault(x => x.Id == id),
                "CompanyFund" => _context.CompanyFunds.FirstOrDefault(x => x.Id == id),
                "CompayExpenses" => _context.CompayExpenses.FirstOrDefault(x => x.Id == id),
                "CareerFile" => _context.CareerFiles.FirstOrDefault(x => x.Id == id),
                "PortfolioTemplate" => _context.PortfolioTemplates.FirstOrDefault(x => x.Id == id),
                "PostModal" => _context.PostModal.FirstOrDefault(x => x.Id == id),
                "ProfilePortfolio" => _context.ProfilePortfolios.FirstOrDefault(x => x.Id == id),
                "VoteCondidate" => _context.VoteCondidates.FirstOrDefault(x => x.Id == id),
                "Setting" => _context.Settings.FirstOrDefault(x => x.Id == id),
                "SuperSetting" => _context.SuperSettings.FirstOrDefault(x => x.Id == id),
                "ProjectMain" => _context.ProjectMains.FirstOrDefault(x => x.Id == id),
                "ProjectFile" => _context.ProjectFiles.FirstOrDefault(x => x.Id == id),
                "ProposalFile" => _context.ProposalFiles.FirstOrDefault(x => x.Id == id),
                "XProjectFile" => _context.XProjectFiles.FirstOrDefault(x => x.Id == id),
                _ => null
            };

            if (obj == null)
                return false;

            var type = obj.GetType();

            var urlProp = type.GetProperty($"{Property}Url") ?? type.GetProperty($"{Property}url");
            var keyProp = type.GetProperty($"{Property}Key") ?? type.GetProperty($"{Property}key");

            if (urlProp != null) urlProp.SetValue(obj, newUrl);
            if (keyProp != null) keyProp.SetValue(obj, key);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
