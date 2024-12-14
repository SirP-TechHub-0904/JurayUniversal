using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.V2.Pages.AuthVadmin
{
    [Authorize(Roles = "mSuperAdmin")]

    public class ListModel : PageModel
    {
       
        private readonly RoleManager<IdentityRole> _roleManager;
      
        public ListModel(RoleManager<IdentityRole> roleManager)
        {
            
            _roleManager = roleManager;

        }

        public List<IdentityRole> Roles = new List<IdentityRole>();
        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            Roles = await _roleManager.Roles.Where(x => x.Name != "mSuperAdmin").ToListAsync();
            return Page();
        }
    }
}
