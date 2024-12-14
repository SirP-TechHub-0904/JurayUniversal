using JurayUniversal.Application.Dtos;
using JurayUniversal.Application.Repository.Services;
using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Pages
{
    public class UHSourceModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly ISettingsService _settingsService;
        public UHSourceModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, ISettingsService settingsService)
        {
            _context = context;
            _settingsService = settingsService;
        }

        public IList<Product> Product { get; set; }
        public SuperSetting SuperSetting { get; set; }
         public Setting Setting { get; set; }
        public string Templatechoose { get; set; }
        public string TitleContent { get; set; }
        public async Task<IActionResult> OnGetAsync(long? id)
        {
            var httpContext = HttpContext;
            VerificationWebDto setting = await _settingsService.ValidateWeb(httpContext);
            if (setting.SettingFound == false)
            {
                return RedirectToPage(setting.Path, new { area = setting.Area });
            }
            if (setting.Portfolio == true)
            {
                return RedirectToPage(setting.PortfolioPath);

            }
            
             SuperSetting = setting.SuperSetting;
             
            Templatechoose = setting.SuperSetting.TemplateLayoutKey;

            Product = await _context.Products.Include(x => x.ProductFeatures).ToListAsync();
            if(id > 0)
            {
                var category = await _context.ProductCategories
                    .Include(x=>x.Product).ThenInclude(x=>x.ProductFeatures)
                    .FirstOrDefaultAsync(x => x.Id == id);
                if(category != null)
                {
                    Product = Product.Where(x => x.ProductCategoryId == id).ToList();
                    TitleContent = "FOR " + category.Title;
                }

            }
            Setting = await _context.Settings.FirstOrDefaultAsync();
             
            return Page();
        }
    }
}
