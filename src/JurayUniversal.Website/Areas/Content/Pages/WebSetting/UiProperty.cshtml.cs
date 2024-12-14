using JurayUniversal.Application.Services.AWS;
using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.Content.Pages.WebSetting
{
       [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,SubAdmin")]

    public class UiPropertyModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;
        private readonly IConfiguration _config;
        private readonly IStorageService _storageService;

        public UiPropertyModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context, IConfiguration config, IStorageService storageService)
        {
            _context = context;
            _config = config;
            _storageService = storageService;
        }

         
        [BindProperty]
        public Setting Setting { get; set; }
        [BindProperty]
        public SuperSetting SuperSetting { get; set; }
         [BindProperty]
        public DataConfig DataConfig { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {

            Setting = await _context.Settings.FirstOrDefaultAsync();

            if (Setting == null)
            {
                return NotFound();
            }
            SuperSetting = await _context.SuperSettings.FirstOrDefaultAsync();

            if (SuperSetting == null)
            {
                return NotFound();
            }
              DataConfig = await _context.DataConfigs.FirstOrDefaultAsync();

            if (DataConfig == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var updatesetting = await _context.Settings.FirstOrDefaultAsync();
            var superupdate = await _context.SuperSettings.FirstOrDefaultAsync();
           var dataConfig = await _context.DataConfigs.FirstOrDefaultAsync();
            ///
            superupdate.TemplateLayoutKey = SuperSetting.TemplateLayoutKey;
            superupdate.LoginTemplateKey = SuperSetting.LoginTemplateKey;
            superupdate.ColorTemplateKey = SuperSetting.ColorTemplateKey;
            superupdate.SliderTemplateKey = SuperSetting.SliderTemplateKey;
            superupdate.BlogTemplateKey = SuperSetting.BlogTemplateKey;
            superupdate.ProductTemplateKey = SuperSetting.ProductTemplateKey;

            superupdate.FooterTemplateKey = SuperSetting.FooterTemplateKey;
            superupdate.PageHeaderTemplateKey = SuperSetting.PageHeaderTemplateKey;
            superupdate.ReloaderTemplateKey = SuperSetting.ReloaderTemplateKey;






            superupdate.ShowMadeByJuray = SuperSetting.ShowMadeByJuray;

            superupdate.UseNormalLogoInLogin = SuperSetting.UseNormalLogoInLogin;
            superupdate.UseWhiteLogoInLogin = SuperSetting.UseWhiteLogoInLogin;
            superupdate.LoginTitle = SuperSetting.LoginTitle;
            superupdate.LoginNoteTitle = SuperSetting.LoginNoteTitle;
            superupdate.LoginNote = SuperSetting.LoginNote;
            superupdate.LoginNoteFooter = SuperSetting.LoginNoteFooter;



            

            try
            {
                _context.Attach(superupdate).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception g) { }
             
            TempData["success"] = "Successful";


            return RedirectToPage("./Index");
        }


    }
}
