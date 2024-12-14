using JurayUniversal.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayUniversal.Website.Areas.Admin.Pages.UserManagement
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "mSuperAdmin")]

    public class RefModel : PageModel
    {
        private readonly UserManager<Profile> _userManager;
        private readonly SignInManager<Profile> _signInManager;
        private readonly ILogger<IndexModel> _logger;
        private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public RefModel(SignInManager<Profile> signInManager,
            ILogger<IndexModel> logger,
            UserManager<Profile> userManager,
            JurayUniversal.Persistence.EF.SQL.DashboardDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }

        [BindProperty]
        public Profile UserDatas { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {

            return Page();
        }

        public int report { get; set; }
        public int report2 { get; set; }
         public async Task<IActionResult> OnPostAsync()
        {

            //if (ModelState.IsValid)
            //{
              report = 0;
              report2 = 0;
            var user = await _userManager.Users.ToListAsync();
               foreach(var x in user)
            {
                //Random random = new Random();

                //int randomNumber = random.Next(100000, 999999);
                //x.RefCode = randomNumber.ToString();
                //await _userManager.UpdateAsync(x);
            }
            return Page();
            
        }

    }
}
