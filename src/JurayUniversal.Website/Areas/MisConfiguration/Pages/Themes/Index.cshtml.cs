using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JurayUniversal.Website.Areas.MisConfiguration.Pages.Themes
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,SubAdmin")]
    public class IndexModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public IndexModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _context = context;
        }

        public SuperSetting SuperSetting { get; set; }
        
        public List<ThemeItem> Themes { get; set; } = new List<ThemeItem>
        {
            new ThemeItem { Name = "Redbiz", Code = "Layout1111", SectionUrl = "RedbizSection", LocalDemo = "/Layout/RedBiz-template/index.html", ExternalLink = "https://redbizwp.themesflat.co/" },
            new ThemeItem { Name = "Carspot", Code = "Layout2222", SectionUrl = "CarspotSection", LocalDemo = "/Layout/carspot/index.html", ExternalLink = "https://templates.scriptsbundle.com/carspot/demos/index.html" },
            new ThemeItem { Name = "Alina", Code = "Layout3333", SectionUrl = "AlinaSection", LocalDemo = "/Layout/Alina/index.html", ExternalLink = "" },
            new ThemeItem { Name = "Archo", Code = "Layout4444", SectionUrl = "ArchoSection", LocalDemo = "/Layout/Alina/index.html", ExternalLink = "https://ui-themez.smartinnovates.net/items/archo/#0" },
            new ThemeItem { Name = "Bizup", Code = "Layout5555", SectionUrl = "BizupSection", LocalDemo = "/Layout/Bizup/index.html", ExternalLink = "https://reactheme.com/products/html/bizup/" },
            new ThemeItem { Name = "Industry", Code = "Layout6666", SectionUrl = "IndustrySection", LocalDemo = "/Layout/Industry/index.html", ExternalLink = "https://zozothemes.com/html/induzy/index-2.html" },
            new ThemeItem { Name = "Lawnella", Code = "Layout7777", SectionUrl = "LawnellaSection", LocalDemo = "/Layout/Lawnella/index.html", ExternalLink = "http://st.ourhtmldemo.com/template/Lawnella/index.html" },
            new ThemeItem { Name = "Mistro", Code = "Layout8888", SectionUrl = "MistroSection", LocalDemo = "/Layout/Mistro/index.html", ExternalLink = "https://themeht.com/template/misto/html/ltr/index.html" },
            new ThemeItem { Name = "Senatory", Code = "Layout9999", SectionUrl = "SenatorySection", LocalDemo = "/Layout/Senatory/index.html", ExternalLink = "https://themeim.com/demo/senatory/index.html#" },
            new ThemeItem { Name = "Education", Code = "Layout11111111", SectionUrl = "EducationSection", LocalDemo = "/Layout/Education/index.html", ExternalLink = "https://kodesolution.com/html/2016/studypress-html/demo/index-mp-layout1.html" },
            new ThemeItem { Name = "Streamlab", Code = "Layout12121212", SectionUrl = "StreamlabSection", LocalDemo = "/Layout/Streamlab/index.html", ExternalLink = "https://template.gentechtreedesign.com/html/streamlab/red-html/index.html" },
            new ThemeItem { Name = "Judge", Code = "Layout13131313", SectionUrl = "JudgeSection", LocalDemo = "", ExternalLink = "https://weblayoutpro.com/judges/index" }
        };

        public async Task<IActionResult> OnGetAsync()
        {
            SuperSetting = await _context.SuperSettings.FirstOrDefaultAsync();
            return Page();
        }
    }

    public class ThemeItem 
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string SectionUrl { get; set; }
        public string LocalDemo { get; set; }
        public string ExternalLink { get; set; }
    }
}
