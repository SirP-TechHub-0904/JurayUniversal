using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.Admin.Pages.AC
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin,Manager")]

    public class EditModel : PageModel
    {
        private readonly UserManager<Profile> _userManager;
        private readonly SignInManager<Profile> _signInManager;
        private readonly ILogger<IndexModel> _logger;

        public EditModel(SignInManager<Profile> signInManager,
            ILogger<IndexModel> logger,
            UserManager<Profile> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }
        [BindProperty]
        public Profile UserDatas { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UserDatas = await _userManager.FindByIdAsync(id);

            if (UserDatas == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
             
            try
            {
                var userupdate = await _userManager.FindByIdAsync(UserDatas.Id);
                
                userupdate.FirstName = UserDatas.FirstName;
                userupdate.LastName = UserDatas.LastName;
                userupdate.Biography = UserDatas.Biography; 
                userupdate.PhoneNumber = UserDatas.PhoneNumber;
                
                userupdate.City = UserDatas.City;
                userupdate.State = UserDatas.State; 
                userupdate.Country = UserDatas.Country; 
                userupdate.DateOfBirth = UserDatas.DateOfBirth; 
                 userupdate.Gender = UserDatas.Gender;
                userupdate.MaritalStatus = UserDatas.MaritalStatus;
                userupdate.UserStatus = UserDatas.UserStatus;

                await _userManager.UpdateAsync(userupdate);
            }
            catch (DbUpdateConcurrencyException)
            {
                
                    throw;
                
            }

            return RedirectToPage("./Index");
        }

    }
}
