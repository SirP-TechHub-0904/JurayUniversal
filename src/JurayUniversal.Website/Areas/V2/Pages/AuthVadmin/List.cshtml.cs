using JurayUniversal.Domain.Models;
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
       
        private readonly RoleManager<AppRole> _roleManager;
      
        public ListModel(RoleManager<AppRole> roleManager)
        {
            
            _roleManager = roleManager;

        }

        public List<AppRole> Roles = new List<AppRole>();
        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            Roles = await _roleManager.Roles.Where(x => x.Name != "mSuperAdmin").ToListAsync();
            return Page();
        }
    }
}
