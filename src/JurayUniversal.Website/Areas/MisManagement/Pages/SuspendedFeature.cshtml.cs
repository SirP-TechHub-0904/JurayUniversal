using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayUniversal.Website.Areas.MisManagement.Pages
{
    [Authorize]
    public class SuspendedFeatureModel : PageModel
    {
        public string FeatureName { get; set; } = string.Empty;

        public void OnGet(string featureName)
        {
            FeatureName = featureName ?? "This Feature";
        }
    }
}
