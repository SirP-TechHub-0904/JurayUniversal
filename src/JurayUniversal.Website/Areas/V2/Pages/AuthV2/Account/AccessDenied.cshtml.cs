using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JurayUniversal.Domain.Models;
using JurayUniversal.Persistence.EF.SQL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.V2.Pages.Authv2.Account
{
    public class AccessDeniedModel : PageModel
    {
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public AccessDeniedModel(DashboardDbContext context)
        {
            _context = context;
        }

        public string TemplateLogin { get; set; }
        public SuperSetting SuperSetting { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var check = await _context.SuperSettings.FirstOrDefaultAsync();
            if (check == null)
            {
                return RedirectToPage("/AuthVadmin/Readonly", new { area = "V2" });
            }
            TemplateLogin = check.LoginTemplateKey;
            SuperSetting = check;
            return Page();
        }
    }


    //private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

    //public IndexModel(JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
    //{
    //    _context = context;
    //}

    //public bool AuthorizeDevice { get; set; }
    //public SuperSetting SuperSetting { get; set; }

    //public async Task OnGetAsync()
    //{
        
    //    SuperSetting = await _context.SuperSettings
    //        .Include(s => s.TemplateCategory).FirstOrDefaultAsync();


    //    return Page();
    //}
}

