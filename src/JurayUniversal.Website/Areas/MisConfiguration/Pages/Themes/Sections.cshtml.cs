using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;

namespace JurayUniversal.Website.Areas.MisConfiguration.Pages.Themes
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,SubAdmin")]
    public class SectionsModel : PageModel
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public SectionsModel(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public List<string> ImagePaths { get; private set; } = new List<string>();
        
        [BindProperty(SupportsGet = true)]
        public string ThemeName { get; set; }

        public void OnGet()
        {
            if (!string.IsNullOrEmpty(ThemeName))
            {
                string webRootPath = _hostingEnvironment.WebRootPath;
                string folderPath = Path.Combine(webRootPath, "templatesample", ThemeName.ToLower());
                
                if (Directory.Exists(folderPath))
                {
                    IEnumerable<string> ImagePath = Directory.GetFiles(folderPath);
                    foreach (var d in ImagePath.ToArray())
                    {
                        string fileName = Path.GetFileName(d);
                        ImagePaths.Add(fileName);
                    }
                    ImagePaths.Sort();
                }
            }
        }
    }
}
