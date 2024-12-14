using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayUniversal.Website.V2.Pages.AuthVadmin
{
    [Authorize(Roles = "mSuperAdmin")]

    public class EditRoleModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public EditRoleModel(RoleManager<IdentityRole> roleManager)
        {

            _roleManager = roleManager;

        }
        [BindProperty]
        public IdentityRole Role { get; set; }
        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
           
            Role = await _roleManager.FindByIdAsync(id);

            if (Role == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }


            try
            {
                await _roleManager.UpdateAsync(Role);
            }
            catch (Exception c)
            {

                throw;

            }

            return RedirectToPage("./List");
        }

    }
}
